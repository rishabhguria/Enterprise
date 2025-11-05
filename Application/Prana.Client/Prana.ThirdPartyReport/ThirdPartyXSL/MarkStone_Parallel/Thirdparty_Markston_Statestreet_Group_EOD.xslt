<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail[FundName='315' or FundName='316' or FundName='318' or FundName='318lcv' or FundName='319' or FundName='319lcv']">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/TaxLotID != TaxLotID)">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">
						<xsl:with-param name="I_TaxLotID">
							<xsl:value-of select="TaxLotID" />
						</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</Groups>
	</xsl:template>


	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_TaxLotID" />

		<xsl:variable name="AllocatedQty" />
		<!-- Building a Group with the EntityID $I_TaxLotID... -->

		<!--Total Quantity-->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
		</xsl:variable>

		<xsl:variable name="QtySum1">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
		</xsl:variable>

		<!--Total Commission-->
		<xsl:variable name="VarCommissionSum">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Side"/>
		</xsl:variable>

		<!--Side-->

		<xsl:variable name="tempTaxlotStateVar">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotStateID>1]/TaxLotState"/>
		</xsl:variable>




		<xsl:variable name="VarTransactionType">
			<xsl:choose>
				<xsl:when test="Side='Buy' and TransactionType='Long Withdrawal'">
					<xsl:value-of select="'FBUY'"/>
				</xsl:when>
				<xsl:when test="Side='Sell' and TransactionType='Long Addition'">
					<xsl:value-of select="'FSELL'"/>
				</xsl:when>
				<xsl:when test="contains(Side,'Buy')">
					<xsl:value-of select="'BUY'"/>
				</xsl:when>
				<xsl:when test="contains(Side,'Sell')">
					<xsl:value-of select="'SELL'"/>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarMessageFunction">
			<xsl:choose>
				<xsl:when test="TaxLotState='Allocated'">
					<xsl:value-of select="'NEWM'"/>

				</xsl:when>

				<xsl:when test="TaxLotState='Deleted' or TaxLotState='Amemded' ">
					<xsl:value-of select="'CANC'"/>

				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarTransactionReference">
			<xsl:value-of select="concat('A',EntityID)"/>
		</xsl:variable>

		<xsl:variable name="VarRelatedReferenceNumber">
			<xsl:choose>

				<xsl:when test="TaxLotState='Deleted' or TaxLotState='Amemded' ">
					<xsl:value-of select="concat('A',EntityID)"/>

				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="PB_NAME" select="'State Street'"/>

		<xsl:variable name = "PRANA_FUND_NAME">
			<xsl:value-of select="FundName"/>
		</xsl:variable>

		<xsl:variable name ="THIRDPARTY_FUND_CODE">
			<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
		</xsl:variable>


		<xsl:variable name="VarFundID">
			<!--<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>-->
			<xsl:value-of select="'ITEV'"/>
		</xsl:variable>

		<xsl:variable name="VarSecurityIDType">

			<xsl:choose>
				<xsl:when test="OSIOptionSymbol!=''">
					<xsl:value-of select="'OSI'"/>
				</xsl:when>
				<xsl:when test="contains(Asset,'Future')">
					<xsl:value-of select="'TS'"/>
				</xsl:when>
				<xsl:when test="CUSIP!=''">
					<xsl:value-of select="'US'"/>
				</xsl:when>
				<xsl:when test="SEDOL!=''">
					<xsl:value-of select="'GB'"/>
				</xsl:when>
				<xsl:when test="ISIN!=''">
					<xsl:value-of select="'ISIN'"/>
				</xsl:when>
				<xsl:when test="Symbol!=''">
					<xsl:value-of select="'TS'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarSecurityID">
			<xsl:choose>
				<xsl:when test="OSIOptionSymbol!=''">
					<xsl:value-of select="OSIOptionSymbol"/>
				</xsl:when>
				<xsl:when test="contains(Asset,'Future')">
					<xsl:value-of select="Symbol"/>
				</xsl:when>
				<xsl:when test="CUSIP!=''">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:when test="SEDOL!=''">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:when test="ISIN!=''">
					<xsl:value-of select="ISIN"/>
				</xsl:when>
				<xsl:when test="Symbol!=''">
					<xsl:value-of select="Symbol"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarSecurityDescription">
			<xsl:value-of select="substring(translate(FullSecurityName,',',''),1,34)"/>
		</xsl:variable>

		<xsl:variable name="VarSecurityType">
			<xsl:choose>
				<xsl:when test="IsSwapped='true'">
					<xsl:value-of select="'TRS'"/>
				</xsl:when>
				<xsl:when test="contains(Asset,'Forward')">
					<xsl:value-of select="'AFWD'"/>
				</xsl:when>
				<xsl:when test="Asset='FX'">
					<xsl:value-of select="'ASET'"/>
				</xsl:when>
				<xsl:when test="contains(Asset,'Option')">
					<xsl:value-of select="'OPT'"/>
				</xsl:when>
				<xsl:when test="Asset='Equity'">
					<xsl:value-of select="'CS'"/>
				</xsl:when>
				<xsl:when test="contains(Asset,'Future')">
					<xsl:value-of select="'FUT'"/>
				</xsl:when>
				<xsl:when test="Asset='FixedIncome'">
					<xsl:value-of select="'CORP'"/>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarCurrencyOfDenomination">
			<xsl:choose>

				<xsl:when test="contains(Asset,'Future')">
					<xsl:value-of select="CurrencySymbol"/>
				</xsl:when>


				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<xsl:variable name="VarStrikePrice">
			<xsl:choose>

				<xsl:when test="number(StrikePrice)">
					<xsl:value-of select="StrikePrice"/>
				</xsl:when>


				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>

			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarExpirationDate">
			<xsl:choose>
				<xsl:when test="Asset='EquityOption'">
					<xsl:value-of select="ExpirationDate"/>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarUnderlyingSecurityIDType">
			<xsl:value-of select="'TS'"/>
		</xsl:variable>

		<xsl:variable name="VarUnderlyingSecurityDesc">
			<xsl:value-of select="substring(translate(FullSecurityName,',',''),1,34)"/>
		</xsl:variable>

		<xsl:variable name="VarMaturityDate">

			<xsl:choose>

				<xsl:when test="contains(Asset,'Future')">
					<xsl:value-of select="ExpirationDate"/>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarInterestRate">
			<xsl:choose>
				<xsl:when test="number(Coupon)">
					<xsl:value-of select="Coupon"/>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>


		</xsl:variable>

		<xsl:variable name="VarDealPriceCode">
			<xsl:value-of select="'ACTU'"/>
		</xsl:variable>

		<xsl:variable name="VarDealPrice">
			<xsl:choose>
				<xsl:when test="number(AveragePrice)">
					<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="Principal" select="AllocatedQty * format-number(AveragePrice,'0.####') * AssetMultiplier"/>
		<xsl:variable name="VarPrincipalAmount">
			<xsl:choose>
				<xsl:when test="number($Principal)">
					<xsl:value-of select="format-number($Principal,'0.##')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="OtherFees">
			<xsl:value-of select="OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + SoftCommissionCharged"/>
		</xsl:variable>
		<xsl:variable name="VarChargesFeesAmount">
			<xsl:choose>
				<xsl:when test="number($OtherFees)">
					<xsl:value-of select="format-number($OtherFees,'0.####')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarAccruedInterestAmount">
			<xsl:choose>
				<xsl:when test="number(AccruedInterest)">
					<xsl:value-of select="format-number(AccruedInterest,'0.##')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="SettleFX">
			<xsl:choose>
				<xsl:when test="number(SettlCurrFxRate)">
					<xsl:value-of select="SettlCurrFxRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varSideMul">
			<xsl:choose>
				<xsl:when test="SideTag = '5' or SideTag = 'C' or SideTag = '2' ">
					<xsl:value-of select="-1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varNetAmmount">
			<xsl:value-of select="$Principal + (($OtherFees + CommissionCharged) * $varSideMul)"/>
		</xsl:variable>
		<xsl:variable name="VarsettleAmount">
			<xsl:choose>
				<xsl:when test="SettlCurrFxRateCalc='M'">
					<xsl:value-of select="format-number(($varNetAmmount * $SettleFX),'0.##')"/>
				</xsl:when>
				<xsl:when test="SettlCurrFxRateCalc='D'">
					<xsl:value-of select="format-number(($varNetAmmount div $SettleFX),'0.##')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($varNetAmmount,'0.##')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarTransactionSubType">
			<xsl:value-of select="'TRAD'"/>
		</xsl:variable>

		<xsl:variable name="VarProcessingIndicator">
			<xsl:choose>
				<xsl:when test="contains(Asset,'Future')">
					<xsl:choose>
						<xsl:when test="contains(Side,'Open')">
							<xsl:value-of select="'OPEP'"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Close')">
							<xsl:value-of select="'CLOP'"/>
						</xsl:when>
						<xsl:when test="contains(Side,'short')">
							<xsl:value-of select="'OPEP'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="contains(Asset,'EquityOption')">
					<xsl:choose>
						<xsl:when test="contains(Side,'Open')">
							<xsl:value-of select="'OPEP'"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Close')">
							<xsl:value-of select="'CLOP'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarSettlementLocation">
			<xsl:value-of select="'DTCYUS33'"/>
		</xsl:variable>

		<xsl:variable name="VarExecutingBrokerIDType">
			<xsl:value-of select="'DTCYID'"/>
		</xsl:variable>

		<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

		<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
			<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
		</xsl:variable>

		<xsl:variable name="Broker">
			<xsl:choose>
				<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
					<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="ExecutingBrokerID">
			<xsl:value-of select="$Broker"/>
		</xsl:variable>

		<xsl:variable name="VarClearingBrokerAgentIDType">
			<xsl:value-of select="'DTCYID'"/>
		</xsl:variable>

		<xsl:variable name="THIRDPARTY_COUNTERPARTY">
			<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
		</xsl:variable>

		<xsl:variable name="BrokerName">
			<xsl:choose>
				<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
					<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarClearingBrokerAgentID">
			<xsl:value-of select="$BrokerName"/>
		</xsl:variable>





		<Group 
				TransactionType ="{$VarTransactionType}" MessageFunction ="{$VarMessageFunction}" TransactionReference="{$VarTransactionReference}" RelatedReferenceNumber="{$VarRelatedReferenceNumber}"
				FundID="{$VarFundID}" TradeDate="{TradeDate}" SettlementDate="{SettlementDate}" LateDeliveryDate=""
				SecurityIDType = "{$VarSecurityIDType}" SecurityID = "{$VarSecurityID}"
				SecurityDescription="{$VarSecurityDescription}" SecurityType="{$VarSecurityType}" CurrencyOfDenomination="{$VarCurrencyOfDenomination}" OptionStyle="" 
				OptionType="{PutOrCall}" ContractSize="{AssetMultiplier}" StrikePrice="{$VarStrikePrice}" ExpirationDate="{$VarExpirationDate}" UnderlyingSecurityIDType="{$VarUnderlyingSecurityIDType}"
				UnderlyingSecurityID="{UnderlyingSymbol}" UnderlyingSecurityDesc="{$VarUnderlyingSecurityDesc}" MaturityDate="{$VarMaturityDate}" IssueDate="{TradeDate}" InterestRate="{$VarInterestRate}" OriginalFace=""
		  Quantity="{$QtySum}" TradeCurrency="{CurrencySymbol}" DealPriceCode="{$VarDealPriceCode}" DealPrice="{$VarDealPrice}" PrincipalAmount="{$VarPrincipalAmount}"
	  CommissionsAmount="{$VarCommissionSum}" ChargesFeesAmount="{$VarChargesFeesAmount}"
	  OtherAmount="" AccruedInterestAmount="{VarAccruedInterestAmount}" TaxesAmount="" StampDutyExemptionAmount="" SettlementCurrency="{SettlCurrency}"
	  SettlementAmount ="{$VarsettleAmount}" TransactionSubType="{$VarTransactionSubType}" SettlementTransactionConditionIndicator="" SettlementTransactionConditionIndicator2="" ProcessingIndicator="{$VarProcessingIndicator}" TrackingIndicator=""
	  SettlementLocation="{$VarSettlementLocation}" PlaceOfTrade="" PlaceOfSafekeeping="" FXContraCurrency="" FXOrderCXLIndicator=""
  ExecutingBrokerIDType ="{$VarExecutingBrokerIDType}" ExecutingBrokerID="{$ExecutingBrokerID}" ExecutingBrokerAcct=""  ClearingBrokerAgentIDType="{$VarClearingBrokerAgentIDType}" ClearingBrokerAgentID="{$VarClearingBrokerAgentID}"
	  ExposureTypeIndicator="" NetMovementIndicator="" NetMovementAmount="" IntermediaryIDType="" IntermediaryID=""
	  AcctWithInstitutionIDType="" AcctWithInstitutionID="" PayingInstitution="" BeneficiaryOfMoney="" CashAcct=""
	  CBO ="" StampDutyExemption="" StampCode="" TRADDETNarrative="" FIANarrative=""
	  ProcessingReference="" ClearingBrokerAccount="" Restrictions="" RepoTermOpenInd="" RepoTermDate=""
	   RepoRateType="" RepoRate="" RepoReference="" RepoTotalTermAmt="" RepoAccrueAmt=""
	  RepoTotalCollCnt ="" RepoCollNumb="" RepoTypeInd="" 
       
       
	  EntityID="{EntityID}" TaxLotState="{$tempTaxlotStateVar}" TaxLotState1="" FileHeader="FALSE" FileFooter="FALSE">

		</Group>
	</xsl:template>
</xsl:stylesheet>