<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->

			<Group 
				transType = "transType" TransStatus="TransStatus" BuySell="BuySell" LongShort="LongShort" PosType="PosType"
				translevel="translevel" ClientRef="ClientRef" Associated="Associated" ExecAccount="ExecAccount" AccountID="{AccountMappedName}"
				ExecBkr="ExecBkr" SecType="SecType" SecID="SecID" desc="desc" TDate="TDate" SDate="SDate"
				SCCY="SCCY"	ExCode="ExCode" qty="qty" 	Price="Price"	type="type" prin="prin" comm="comm" comtype="comtype" 
				Taxfees="Taxfees" Tax2="Tax2" feesind="feesind" interest="interest" interestindicator="interestindicator"
				netamount="netamount" hsyind="hsyind" custbkr="custbkr" mmgr="mmgr" bookid="bookid"
				dealid="dealid" taxlotid="taxlotid" taxdate="taxdate" taxprice="taxprice" closeoutmethod="closeoutmethod" exrate="exrate"
				acqdate="acqdate" comments="comments"
				EntityID="" TaxLotState="" IsCaptionChangeRequired="true" RowHeader="false"/>

			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[IsSwapped!='true' and not(contains(Asset,'FX')) and not(contains(Asset,'EquityOption') and contains(Symbol,'-')) and contains(CounterParty,'CROSS')!='true']">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">
						<xsl:with-param name="I_GroupID">
							<xsl:value-of select="PBUniqueID" />
						</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</Groups>
	</xsl:template>


	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />
		<xsl:variable name="AllocatedQty" />
		<!-- Building a Group with the EntityID $I_GroupID... -->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/AllocatedQty)"/>
		</xsl:variable>
		<xsl:variable name="GroupNetAmt">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/NetAmount)"/>
		</xsl:variable>
		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="TaxOnCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TaxOnCommissions)"/>
		</xsl:variable>
		<xsl:variable name="SecFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SecFee)"/>
		</xsl:variable>
		<xsl:variable name="StampDutySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/StampDuty)"/>
		</xsl:variable>
		<xsl:variable name="TransactionLevySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TransactionLevy)"/>
		</xsl:variable>
		<xsl:variable name="ClearingFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/ClearingFee)"/>
		</xsl:variable>
		<xsl:variable name="MiscFeesSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/MiscFees)"/>
		</xsl:variable>
		<xsl:variable name="OrfFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OrfFee)"/>
		</xsl:variable>
		<xsl:variable name="OtherBrokerFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OtherBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="GrossAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/GrossAmount)"/>
		</xsl:variable>
		<xsl:variable name="NetAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/NetAmount)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<!--<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][FromDeleted != 'Yes']/Side"/>-->
			<xsl:value-of  select="Side"/>
		</xsl:variable>

		<xsl:variable name="tempCPVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CounterParty"/>
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


		<xsl:variable name="COMM">
			<xsl:choose>
				<xsl:when test="SettlCurrFxRate=0">
					<xsl:value-of select="$CommissionSum"/>
				</xsl:when>

				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
					<xsl:value-of select="$CommissionSum * SettlCurrFxRate"/>
				</xsl:when>

				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
					<xsl:value-of select="$CommissionSum div SettlCurrFxRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
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

		<xsl:variable name="GrossAmnt">
			<xsl:choose>
				<xsl:when test="SettlCurrFxRate=0">
					<xsl:value-of select="$GroupGrsAmt"/>
				</xsl:when>
				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
					<xsl:value-of select="$GroupGrsAmt * SettlCurrFxRate"/>
				</xsl:when>

				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
					<xsl:value-of select="$GroupGrsAmt div SettlCurrFxRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupNetAmt1">
			<xsl:choose>
				<xsl:when test="SettlCurrFxRate=0">
					<xsl:value-of select="$GroupNetAmt"/>
				</xsl:when>
				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
					<xsl:value-of select="$GroupNetAmt * SettlCurrFxRate"/>
				</xsl:when>

				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
					<xsl:value-of select="$GroupNetAmt div SettlCurrFxRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
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

		<xsl:variable name="GroupNetAmt2">
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

		<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

		<xsl:variable name="THIRDPARTY_FUND_NAME">
			<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
		</xsl:variable>

		<xsl:variable name="AccountId">
			<xsl:choose>
				<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
					<xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PRANA_FUND_NAME"/>
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

		<!--<xsl:variable name="PRANA_FUND_NAME_Group" select="AccountName"/>

		<xsl:variable name="THIRDPARTY_FUND_NAME_Group">
			<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME_Group]/@PBFundCode"/>
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

		<xsl:variable name="COMM2" select="format-number(CommissionCharged,'###.00')"/>
		<xsl:variable name="Commission">
			<xsl:choose>
				<xsl:when test="SettlCurrFxRate=0">
					<xsl:value-of select="$COMM2"/>
				</xsl:when>
				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
					<xsl:value-of select="$COMM2 * SettlCurrFxRate"/>
				</xsl:when>

				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
					<xsl:value-of select="$COMM2 div SettlCurrFxRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="Custodian">
			<xsl:choose>
				<xsl:when test="contains(AccountName,'BAML')">
					<xsl:value-of select="'BAML'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'GSCO'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupStamp">
			<xsl:value-of select="$GroupGrsAmt * 0.0000184"/>
		</xsl:variable>

		<xsl:variable name="GroupOrf">
			<xsl:value-of select="$QtySum * 0.04"/>
		</xsl:variable>

		<xsl:variable name="GroupTotalFees">
			<xsl:choose>
				<xsl:when test="number($SecFeeSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) &gt; 0">
					<xsl:value-of select="($SecFeeSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum)"/>
				</xsl:when>
				<xsl:when test="number($SecFeeSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) &lt; 0">
					<xsl:value-of select="($SecFeeSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) * -1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<xsl:variable name="GroupTotalFees1">
			<xsl:choose>
				<xsl:when test="SettlCurrFxRate=0">
					<xsl:value-of select="$GroupTotalFees"/>
				</xsl:when>
				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
					<xsl:value-of select="$GroupTotalFees * SettlCurrFxRate"/>
				</xsl:when>

				<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
					<xsl:value-of select="$GroupTotalFees div SettlCurrFxRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="AveragePrice">
			<xsl:choose>
				<xsl:when test="SettlCurrAmt=0">
					<xsl:value-of select="AveragePrice"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="SettlCurrAmt"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="SettlCurrFxRate">
			<xsl:choose>
				<xsl:when test="SettlCurrency='USD'">
					<xsl:value-of select="'1'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="SettlCurrFxRate"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<Group 
				transType = "TR001" TransStatus="{$varTaxlotStateTx}" BuySell="" LongShort="" PosType="{$Sidevar}"
				translevel="B" ClientRef="{PBUniqueID}" Associated="{PBUniqueID}" ExecAccount="038076691" AccountID="038076691"
				ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{translate(FullSecurityName,',','')}" TDate="{TradeDate}" SDate="{SettlementDate}"
				SCCY="{SettlCurrency}"	ExCode="" qty="{$QtySum}" 	Price="{$AveragePrice}"	type="G" prin="{$GrossAmnt}" 
				comm="{format-number($COMM,'###.00')}" comtype="F" 
				Taxfees="{$GroupTotalFees1}" 
				Tax2="0"
				feesind="F" interest="0" interestindicator="" netamount="{format-number($GroupNetAmt1,'###.00')}" hsyind="N" custbkr="MSCO" mmgr="" bookid=""
				dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="{$SettlCurrFxRate}" acqdate="" comments=""
				EntityID="{EntityID}" TaxLotState="{TaxLotState}" IsCaptionChangeRequired="true" RowHeader="false">


			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
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

				<xsl:variable name="TaxlotGRS" select="AllocatedQty * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
				<xsl:variable name="TaxlotGrsAmt">
					<xsl:choose>
						<xsl:when test="SettlCurrFxRate=0">
							<xsl:value-of select="$TaxlotGRS"/>
						</xsl:when>
						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
							<xsl:value-of select="$TaxlotGRS * SettlCurrFxRate"/>
						</xsl:when>

						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
							<xsl:value-of select="$TaxlotGRS div SettlCurrFxRate"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
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
					<xsl:value-of select="($TaxlotGrsAmt + (SecFee + CommissionCharged + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions) * $TaxlotSideMultiplier)"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME_T" select="AccountName"/>

				<xsl:variable name="THIRDPARTY_FUND_NAME_T">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME_T]/@PBFundCode"/>
				</xsl:variable>

				<xsl:variable name="AccountId_T">
					<xsl:choose>
						<xsl:when test="$THIRDPARTY_FUND_NAME_T!=''">
							<xsl:value-of select="$THIRDPARTY_FUND_NAME_T"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PRANA_FUND_NAME_T"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="Stamp">
					<xsl:value-of select="GrossAmount * 0.0000184"/>
				</xsl:variable>

				<xsl:variable name="Orf">
					<xsl:value-of select="AllocatedQty * 0.04"/>
				</xsl:variable>

				<xsl:variable name="TaxFees">
					<xsl:choose>
						<xsl:when test="number(SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) &gt; 0">
							<xsl:value-of select="SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee"/>
						</xsl:when>
						<xsl:when test="number(SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) &lt; 0">
							<xsl:value-of select="(SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) * -1"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="TAXFEES">
					<xsl:choose>
						<xsl:when test="SettlCurrFxRate=0">
							<xsl:value-of select="$TaxFees"/>
						</xsl:when>
						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
							<xsl:value-of select="$TaxFees * SettlCurrFxRate"/>
						</xsl:when>

						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
							<xsl:value-of select="$TaxFees div SettlCurrFxRate"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>


				<xsl:variable name="COMM1" select="format-number(CommissionCharged,'###.00')"/>
				<xsl:variable name="Commission1">
					<xsl:choose>
						<xsl:when test="SettlCurrFxRate=0">
							<xsl:value-of select="$COMM1"/>
						</xsl:when>
						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
							<xsl:value-of select="$COMM1 * SettlCurrFxRate"/>
						</xsl:when>

						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
							<xsl:value-of select="$COMM1 div SettlCurrFxRate"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="NTANT" select="format-number(NetAmount,'###.00')"/>
				<xsl:variable name="AMNT">
					<xsl:choose>
						<xsl:when test="SettlCurrFxRate=0">
							<xsl:value-of select="$NTANT"/>
						</xsl:when>
						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
							<xsl:value-of select="$NTANT * SettlCurrFxRate"/>
						</xsl:when>

						<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
							<xsl:value-of select="$NTANT div SettlCurrFxRate"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="exRate">
					<xsl:choose>
						<xsl:when test="SettlCurrency='USD'">
							<xsl:value-of select="'1'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="SettlCurrFxRate"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="amp">N</xsl:variable>

				<ThirdPartyFlatFileDetail
							TaxLotState="{TaxLotState}" transType = "TR001" TransStatus="{$varTaxlotState}" BuySell="" LongShort="" PosType="{$Sidevar}"
							translevel="A" ClientRef="{concat(substring(EntityID,3),'C')}" Associated="{PBUniqueID}" ExecAccount="038076691" AccountID="{AccountMappedName}"
							ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{translate(FullSecurityName,',','')}" TDate="{TradeDate}" SDate="{SettlementDate}"
							SCCY="{SettlCurrency}"	ExCode="" qty="{AllocatedQty}" Price="{$AveragePrice}" type="G" prin="{$TaxlotGrsAmt}"
							comm="{$Commission1}" comtype="F"
							Taxfees="{$TAXFEES}" 
							Tax2="0"
							feesind="F" interest="0" interestindicator="" netamount="{$AMNT}" hsyind="N" custbkr="MSCO" mmgr="" bookid=""
							dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="{$exRate}" acqdate="" comments=""
							EntityID="{EntityID}" IsCaptionChangeRequired="true" RowHeader="false"/>
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>