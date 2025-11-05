<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[contains(Asset,'FX') and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
					<xsl:if test="(AccountName='Sheffield Partners LP-JPM' or AccountName='Sheffield Partners LP-MS')">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">
						<xsl:with-param name="I_GroupID">
							<xsl:value-of select="PBUniqueID" />
						</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</Groups>
	</xsl:template>


	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />
		<xsl:variable name="AllocatedQty" />
		<!-- Building a Group with the EntityID $I_GroupID... -->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/AllocatedQty)"/>
		</xsl:variable>
		<xsl:variable name="GroupNetAmt">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/NetAmount)"/>
		</xsl:variable>
		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/CommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="TaxOnCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/TaxOnCommissions)"/>
		</xsl:variable>
		<xsl:variable name="SecFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/SecFee)"/>
		</xsl:variable>
		<xsl:variable name="StampDutySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/StampDuty)"/>
		</xsl:variable>
		<xsl:variable name="TransactionLevySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/TransactionLevy)"/>
		</xsl:variable>
		<xsl:variable name="ClearingFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/ClearingFee)"/>
		</xsl:variable>
		<xsl:variable name="MiscFeesSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/MiscFees)"/>
		</xsl:variable>
		<xsl:variable name="OrfFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/OrfFee)"/>
		</xsl:variable>
		<xsl:variable name="OtherBrokerFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/OtherBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="GrossAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/GrossAmount)"/>
		</xsl:variable>
		<xsl:variable name="NetAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/NetAmount)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/Side"/>
		</xsl:variable>

		<xsl:variable name="tempCPVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/CounterParty"/>
		</xsl:variable>

		<xsl:variable name="PB_NAME" select="'MS'"/>

		<xsl:variable name="PB_COUNTERPARTY_NAME">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$tempCPVar]/@ThirdPartyBrokerID"/>
		</xsl:variable>

		<xsl:variable name="CPVar">
			<!--<xsl:choose>
				<xsl:when test="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
				<xsl:when test="$tempCPVar='SAND'">SDLR</xsl:when>
				<xsl:when test="$tempCPVar='ISGI'">INTS</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="$tempCPVar"/>
				</xsl:otherwise>
			</xsl:choose>-->
			<xsl:choose>
				<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
					<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$tempCPVar"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="Sidevar">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">BL</xsl:when>
				<xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">BC</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">SL</xsl:when>
				<xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">SS</xsl:when>
				<xsl:otherwise></xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tempTaxlotStateVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotStateID>1]/TaxLotState"/>
		</xsl:variable>

		<xsl:variable name="varTaxlotStateGrp">
			<xsl:choose>
				<xsl:when test="$tempTaxlotStateVar != ''">COR</xsl:when>
				<xsl:otherwise>NEW</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<!--<xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
					<xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>
					<xsl:value-of select="concat($varSymbolBef,'/',$varSymbolAft)"/>-->

		<!--<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test="Asset='EquityOption'">
					
					<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>					
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

		<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<xsl:value-of select="OSIOptionSymbol"/>
				</xsl:when>

				<xsl:when test="SEDOL != ''">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:when test="CUSIP != ''">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:when test="ISIN != ''">
					<xsl:value-of select="ISIN"/>
				</xsl:when>


				<xsl:when test="Symbol != ''">
					<xsl:value-of select="Symbol"/>
				</xsl:when>
				<xsl:when test="RIC != ''">
					<xsl:value-of select="RIC"/>
				</xsl:when>
				<xsl:when test="BBCode != ''">
					<xsl:value-of select="BBCode"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varSecType">
			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<xsl:value-of select="'T'"/>
				</xsl:when>

				<xsl:when test="SEDOL != ''">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="CUSIP != ''">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="ISIN != ''">
					<xsl:value-of select="'I'"/>
				</xsl:when>


				<xsl:when test="Symbol != ''">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="RIC != ''">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="BBCode != ''">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="'T'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupGrsAmt">
			<xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
		</xsl:variable>

		<xsl:variable name="GroupSideMultiplier">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open' or $tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">
					<xsl:value-of select="1"/>
				</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close' or $tempSideVar='Sell short' or $tempSideVar='Sell to Open'">
					<xsl:value-of select="-1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupNetAmt1">
			<xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
		</xsl:variable>

		<xsl:variable name="PRANA_EXCHANGE" select="Exchange"/>

		<xsl:variable name="PB_EXCHANGE">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE]/@PBExchangeName"/>
		</xsl:variable>

		<xsl:variable name="varEXCODE">
			<xsl:choose>
				<xsl:when test ="$PB_EXCHANGE!=''">
					<xsl:value-of select="$PB_EXCHANGE"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Exchange"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varTaxlotStateTx">
			<xsl:choose>
				<xsl:when test="TaxLotState='Allocated'">
					<xsl:value-of select ="'NEW'"/>
				</xsl:when>
				<xsl:when test="TaxLotState='Amended'">
					<xsl:value-of select ="'COR'"/>
				</xsl:when>
				<xsl:when test="TaxLotState='Deleted'">
					<xsl:value-of select ="'CAN'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--<xsl:variable name="PRANA_FUND_NAME_Group" select="FundName"/>

		<xsl:variable name="THIRDPARTY_FUND_NAME_Group">
			<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME_Group]/@PBFundCode"/>
		</xsl:variable>

		<xsl:variable name="AccountId_Group">
			<xsl:choose>
				<xsl:when test="$THIRDPARTY_FUND_NAME_Group!=''">
					<xsl:value-of select="$THIRDPARTY_FUND_NAME_Group"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PRANA_FUND_NAME_Group"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

		<xsl:variable name="Custodian">
			<xsl:choose>
				<xsl:when test="AccountMappedName='038Q51978'">
					<xsl:value-of select="'MSCO'"/>
				</xsl:when>
				<xsl:when test="AccountMappedName='10238924'">
					<xsl:value-of select="'JPME'"/>
				</xsl:when>

				<!--<xsl:when test="contains(AccountName,'BAML')">
					<xsl:value-of select="'BAML'"/>
				</xsl:when>
				<xsl:when test="contains(AccountName,'Quantum')">
					<xsl:value-of select="'MSCO'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'GSCO'"/>
				</xsl:otherwise>-->
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupStamp">
			<xsl:value-of select="$GroupGrsAmt * 0.0000184"/>
		</xsl:variable>

		<xsl:variable name="GroupOrf">
			<xsl:value-of select="$QtySum * 0.04"/>
		</xsl:variable>

    <xsl:variable name="varTransactionStatus">
      <xsl:choose>
        <xsl:when test="TaxLotState='Allocated'">
          <xsl:value-of select ="'NEW'"/>
        </xsl:when>
        <xsl:when test="TaxLotState='Amended'">
          <xsl:value-of select ="'COR'"/>
        </xsl:when>
        <xsl:when test="TaxLotState='Deleted'">
          <xsl:value-of select ="'CAN'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!--<xsl:variable name="PB_NAME" select="'G2'"/>-->
		
		<xsl:variable name ="varIntrumenttype">
			<xsl:choose>
				<xsl:when test="IsSwapped = 'true'">
					<xsl:value-of select ="'SWAP'"/>
				</xsl:when>
				<xsl:when test="Asset = 'FX'">
					<xsl:value-of select ="'SPOT'"/>
				</xsl:when>
				<xsl:when test="Asset = 'FXForward'">
					<xsl:value-of select ="'FORWARD'"/>
				</xsl:when>
			</xsl:choose>
			
		</xsl:variable>

		<xsl:variable name="varBuyCurrency">
			<xsl:choose>
				<xsl:when test="Side='Buy'">
					<xsl:value-of select="LeadCurrencyName"/>
				</xsl:when>
				<xsl:when test="Side='Sell'">
					<xsl:value-of select="VsCurrencyName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="varSellCurrency">
			<xsl:choose>
				<xsl:when test="Side='Buy'">
					<xsl:value-of select="VsCurrencyName"/>
				</xsl:when>
				<xsl:when test="Side='Sell'">
					<xsl:value-of select="LeadCurrencyName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varSellQuantity">
			<xsl:choose>
				<xsl:when test="Side='Sell'">
					<xsl:value-of select="format-number($QtySum,'0.##')"/>
				</xsl:when>
				<xsl:when test="Side='Buy'">

					<xsl:value-of select="format-number(($QtySum * AveragePrice),'0.##')"/>

				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="TaxLotState='Deleted'">
							<xsl:choose>
								<xsl:when test="Side='Sell'">
									<xsl:value-of select="format-number($QtySum,'0.##')"/>
								</xsl:when>
								<xsl:when test="Side='Buy'">

									<xsl:value-of select="format-number(($QtySum * AveragePrice),'0.##')"/>

								</xsl:when>
							</xsl:choose>
						</xsl:when>
					</xsl:choose>


				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varBuyQuantity">
			<xsl:choose>
				<xsl:when test="Side='Buy'">
					<xsl:value-of select="$QtySum"/>
				</xsl:when>
				<xsl:when test="Side='Sell'">

					<xsl:value-of select="$QtySum * AveragePrice"/>

				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="TaxLotState='Deleted'">
							<xsl:choose>
								<xsl:when test="Side='Buy'">
									<xsl:value-of select="$QtySum"/>
								</xsl:when>
								<xsl:when test="Side='Sell'">

									<xsl:value-of select="$QtySum * AveragePrice"/>

								</xsl:when>
							</xsl:choose>
						</xsl:when>
					</xsl:choose>


				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<xsl:variable name="FXRate">
			<xsl:choose>
				<xsl:when test="number(FXRate_Taxlot)">
					<xsl:value-of select="FXRate_Taxlot"/>
				</xsl:when>
				<xsl:when test="number(ForexRate)">
					<xsl:value-of select="ForexRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varDealtCurr">

			<xsl:choose>
				<xsl:when test ="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">

					<xsl:value-of select="LeadCurrencyName"/>

				</xsl:when>
				<xsl:when test ="Side='Sell' or Side='Sell to Open' or Side='Sell short' or Side='Sell to Close'">

					<xsl:value-of select="VsCurrencyName"/>
				</xsl:when>
				<xsl:otherwise>

					<xsl:value-of select="''"/>

				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarAvgPrice">
			<xsl:choose>
				<xsl:when test ="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">

					<xsl:value-of select="format-number(AveragePrice,'0.######')"/>

				</xsl:when>
				<xsl:when test ="Side='Sell' or Side='Sell to Open' or Side='Sell short' or Side='Sell to Close'">

					<xsl:value-of select="format-number((1 div AveragePrice),'0.######')"/>
				</xsl:when>
				<xsl:otherwise>

					<xsl:value-of select="''"/>

				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="VarSettlementDate">
			<xsl:choose>
				<xsl:when test="Asset= 'FXForward'">
					<xsl:value-of select="ExpirationDate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="SettlementDate"/>
				</xsl:otherwise>
			</xsl:choose>
			
		</xsl:variable>
		
		<Group
        
					TransactionType = "FX002" TransactionStatus="{$varTransactionStatus}" TransactionLevel="B" Product_Intrumenttype="{$varIntrumenttype}" ClientTraqdeRefNo="{PBUniqueID}"
						AssociatedTrade_RequestId="{PBUniqueID}" ExecutionAccount="038308300" AccountId_Fund="038308300" ExecutingBroker="{$CPVar}" TradeDate="{TradeDate}"
						ValueDate="{$VarSettlementDate}" BuyQuantity="{$varBuyQuantity}" SellQuantity="{$varSellQuantity}" BuyCCY="{$varBuyCurrency}" SellCCY="{$varSellCurrency}" DealtCurrency="{$varDealtCurr}"
						Rate="{$VarAvgPrice}"	NdfFlag="FALSE" NdfFixingDate="" 	NdfLinkedTrade=""	PB="MSCO" FarValueDate=""
						FarValueRate="" ClientBaseEquivalent=""
						HedgeorSpeculative=""
						TaxIndicator=""
						HearsayInd="Y" Custodian="{$Custodian}" ManagerMoney="" DealId="" AcquisitionDate="" Comments="" TradeType="" Reporter=""
						EntityID="{EntityID}" TaxLotState="{TaxLotState}">

			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and CounterParty != 'Undefined' and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset' and contains(AccountName,'Quantum Partners LP')!='true']">
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>

				<xsl:variable name="varTaxlotState">
					<xsl:choose>
						<xsl:when test="TaxLotState='Allocated'">
							<xsl:value-of select ="'NEW'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Amended'">
							<xsl:value-of select ="'COR'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Deleted'">
							<xsl:value-of select ="'CAN'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="TaxlotGrsAmt">
					<xsl:value-of select="AllocatedQty * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
				</xsl:variable>

				<xsl:variable name="TaxlotSideMultiplier">
					<xsl:choose>
						<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open' or $tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">
							<xsl:value-of select="1"/>
						</xsl:when>
						<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close' or $tempSideVar='Sell short' or $tempSideVar='Sell to Open'">
							<xsl:value-of select="-1"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="TaxlotNetAmt">
					<xsl:value-of select="$TaxlotGrsAmt + ((CommissionCharged + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions) * $TaxlotSideMultiplier)"/>
				</xsl:variable>

			
				<xsl:variable name="Stamp">
					<xsl:value-of select="GrossAmount * 0.0000184"/>
				</xsl:variable>

				<xsl:variable name="Orf">
					<xsl:value-of select="AllocatedQty * 0.04"/>
				</xsl:variable>

				<xsl:variable name="TaxFees">
					<xsl:choose>
						<xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) &gt; 0">
							<xsl:value-of select="StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee"/>
						</xsl:when>
						<xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) &lt; 0">
							<xsl:value-of select="(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) * -1"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="varBuyQuantity_Alloc">
				
							<xsl:choose>
								<xsl:when test="Side='Buy'">
									<xsl:value-of select="AllocatedQty"/>
								</xsl:when>
								<xsl:when test="Side='Sell'">								
									<xsl:value-of select="AllocatedQty * AveragePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
					
					
					
				</xsl:variable>

				<xsl:variable name="varSellQuantity_Alloc">
					<xsl:choose>
						<xsl:when test="Side='Sell'">
							<xsl:value-of select="format-number(AllocatedQty,'0.##')"/>
						</xsl:when>
						<xsl:when test="Side='Buy'">
							<xsl:value-of select="format-number((AllocatedQty * AveragePrice),'0.##')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<!--<xsl:variable name="varSellQuantity_Alloc">
					
							<xsl:choose>
<xsl:when test="FXConversionMethodOperator_Taxlot='M'">
<xsl:value-of select="AllocatedQty * $FXRate"/>
</xsl:when>
<xsl:when test="FXConversionMethodOperator_Taxlot='D'">
<xsl:value-of select="AllocatedQty div $FXRate"/>
</xsl:when>
</xsl:choose>
							
				</xsl:variable>-->

				<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

				<xsl:variable name="THIRDPARTY_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
				</xsl:variable>

				<xsl:variable name="AccountId_Alloc">
					<xsl:choose>
						<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
							<xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PRANA_FUND_NAME"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="VarClientTraqdeRedNo">
					<xsl:value-of select="concat(PBUniqueID,'038Q51978')"/>
					<!--<xsl:value-of select="concat(PBUniqueID,$AccountId_Alloc)"/>-->
				</xsl:variable>


				<xsl:variable name="amp">N</xsl:variable>

				<ThirdPartyFlatFileDetail
							TaxLotState="{TaxLotState}" TransactionType = "FX002" TransactionStatus="{$varTransactionStatus}" TransactionLevel="A" Product_Intrumenttype="{$varIntrumenttype}" ClientTraqdeRefNo="{$VarClientTraqdeRedNo}"
						AssociatedTrade_RequestId="{PBUniqueID}" ExecutionAccount="038308300" AccountId_Fund="{'038Q51978'}" ExecutingBroker="{$CPVar}" TradeDate="{TradeDate}"
						ValueDate="{$VarSettlementDate}" BuyQuantity="{$varBuyQuantity_Alloc}" SellQuantity="{$varSellQuantity_Alloc}" BuyCCY="{$varBuyCurrency}" SellCCY="{$varSellCurrency}" DealtCurrency="{$varDealtCurr}"
						Rate="{$VarAvgPrice}"	NdfFlag="FALSE" NdfFixingDate="" 	NdfLinkedTrade=""	PB="MSCO" FarValueDate=""
						FarValueRate="" ClientBaseEquivalent=""
						HedgeorSpeculative=""
						TaxIndicator=""
						HearsayInd="Y" Custodian="{$Custodian}" ManagerMoney="" DealId="" AcquisitionDate="" Comments="" TradeType="" Reporter=""
							EntityID="{EntityID}" />
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>