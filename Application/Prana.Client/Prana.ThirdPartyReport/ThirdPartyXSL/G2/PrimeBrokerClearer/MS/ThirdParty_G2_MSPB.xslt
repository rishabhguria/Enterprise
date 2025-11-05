<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->

			<!--<Group 
				
				
				
				transType = "transType" TransStatus="TransStatus" BuySell="BuySell" LongShort="LongShort" PosType="PosType"
			translevel="translevel" ClientRef="ClientRef" Associated="Associated" ExecAccount="ExecAccount" AccountID="AccountID"
			ExecBkr="ExecBkr" SecType="SecType" SecID="SecID" desc="desc" TDate="TDate" SDate="SDate"
			SCCY="SCCY"	ExCode="ExCode" qty="qty" 	Price="Price"	type="type" prin="prin" comm="comm" comtype="comtype" 
			Taxfees="Taxfees" Tax2="Tax2" feesind="feesind" interest="interest" interestindicator="interestindicator"
			netamount="netamount" hsyind="hsyind" custbkr="custbkr" mmgr="mmgr" bookid="bookid"
			dealid="dealid" taxlotid="taxlotid" taxdate="taxdate" taxprice="taxprice" closeoutmethod="closeoutmethod" exrate="exrate"
			acqdate="acqdate" comments="comments" fxind=""
			EntityID="" TaxLotState="" IsCaptionChangeRequired="true" />
			
			
				--><!--transType = "TR001" TransStatus="{$varTaxlotStateTx}" BuySell="" LongShort="" PosType="{$Sidevar}"
			translevel="B" ClientRef="{PBUniqueID}" Associated="{PBUniqueID}" ExecAccount="038098638" AccountID="038098638"
			ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
			SCCY="{CurrencySymbol}"	ExCode="" qty="{$QtySum}" 	Price="{format-number(AveragePrice,'###.0000')}"	type="G" prin="{format-number($GroupGrsAmt,'###.00')}"
			comm="{format-number($CommissionSum,'###.00')}" comtype="F"
			Taxfees="{$GroupTotalFees}"
			Tax2="0"
			feesind="F" interest="0" interestindicator="" netamount="{format-number($GroupNetAmt,'###.00')}" hsyind="N" custbkr="{$Custodian}" mmgr="" bookid=""
			dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" comments="" fxind="" RowHeader="true"
			EntityID="" TaxLotState="">-->
			

			

			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
					<xsl:if test="CounterParty != 'Undefined'">
						<xsl:if test="Asset !='FX'">
						<!-- ...buid a Group for this node_id -->
						<xsl:call-template name="TaxLotIDBuilder">
							<xsl:with-param name="I_GroupID">
								<xsl:value-of select="PBUniqueID" />
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</Groups>
	</xsl:template>


	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />

		<xsl:if test="TaxLotState != 'Deleted'">

			<xsl:variable name="AllocatedQty" />
			<!-- Building a Group with the EntityID $I_GroupID... -->
			<xsl:variable name="QtySum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
			</xsl:variable>
			<xsl:variable name="GroupNetAmt">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
			</xsl:variable>
			<xsl:variable name="CommissionSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
			</xsl:variable>
			<xsl:variable name="TaxOnCommissionSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TaxOnCommissions)"/>
			</xsl:variable>
			<xsl:variable name="SecFeeSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SecFee)"/>
			</xsl:variable>
			<xsl:variable name="StampDutySum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/StampDuty)"/>
			</xsl:variable>
			<xsl:variable name="TransactionLevySum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TransactionLevy)"/>
			</xsl:variable>
			<xsl:variable name="ClearingFeeSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ClearingFee)"/>
			</xsl:variable>
			<xsl:variable name="MiscFeesSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/MiscFees)"/>
			</xsl:variable>
			<xsl:variable name="OrfFeeSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OrfFee)"/>
			</xsl:variable>
			<xsl:variable name="OtherBrokerFeeSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OtherBrokerFee)"/>
			</xsl:variable>
			<xsl:variable name="GrossAmountSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/GrossAmount)"/>
			</xsl:variable>
			<xsl:variable name="NetAmountSum">
				<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
			</xsl:variable>
			<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
			<xsl:variable name="tempSideVar">
				<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
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
					<xsl:when test="contains(AccountName,'BAML')">
						<xsl:value-of select="'BAML'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'MSCO'"/>
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
					<xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) &gt; 0">
						<xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum)"/>
					</xsl:when>
					<xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) &lt; 0">
						<xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) * -1"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:variable>

			<Group 
					transType = "TR001" TransStatus="{$varTaxlotStateTx}" BuySell="" LongShort="" PosType="{$Sidevar}"
					translevel="B" ClientRef="{PBUniqueID}" Associated="{PBUniqueID}" ExecAccount="038098638" AccountID="038098638"
					ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
					SCCY="{CurrencySymbol}"	ExCode="" qty="{$QtySum}" 	Price="{format-number(AveragePrice,'###.0000')}"	type="G" prin="{format-number($GroupGrsAmt,'###.00')}" 
					comm="{format-number($CommissionSum,'###.00')}" comtype="F" 
					Taxfees="{$GroupTotalFees}" 
					Tax2="0"
					feesind="F" interest="0" interestindicator="" netamount="{format-number($GroupNetAmt,'###.00')}" hsyind="N" custbkr="{$Custodian}" mmgr="" bookid=""
					dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" comments="" fxind="" RowHeader="true"
					EntityID="{EntityID}" TaxLotState="{TaxLotState}">


				<!-- ...selecting all the records for this Group... -->
				<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
					<!-- ...and building a ThirdPartyFlatFileDetail for each -->
					<!--<xsl:if test="TaxLotState!='Deleted'">-->

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

					<xsl:variable name="PRANA_FUND_NAME" select="AccountMappedName"/>

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

					<xsl:variable name="amp">N</xsl:variable>

					<ThirdPartyFlatFileDetail
								TaxLotState="{TaxLotState}" transType = "TR001" TransStatus="{$varTaxlotState}" BuySell="" LongShort="" PosType="{$Sidevar}"
								translevel="A" ClientRef="{concat(PBUniqueID,$AccountId)}" Associated="{PBUniqueID}" ExecAccount="038098638" AccountID="{$AccountId}"
								ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
								SCCY="{CurrencySymbol}"	ExCode="" qty="{AllocatedQty}" Price="{format-number(AveragePrice,'###.0000')}" type="G" prin="{format-number($TaxlotGrsAmt,'###.00')}"
								comm="{format-number(CommissionCharged,'###.00')}" comtype="F"
								Taxfees="{$TaxFees}" 
								Tax2="0"
								feesind="F" interest="0" interestindicator="" netamount="{format-number(NetAmount,'###.00')}" hsyind="N" custbkr="{$Custodian}" mmgr="" bookid=""
								dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" comments="" fxind="" RowHeader="true"
								EntityID="{EntityID}" />
					<!--</xsl:if>-->
				</xsl:for-each>

			</Group>
		</xsl:if>




		<xsl:if test="TaxLotState = 'Deleted'">

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
				<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
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
					<xsl:when test="contains(AccountName,'BAML')">
						<xsl:value-of select="'BAML'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'MSCO'"/>
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
					<xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) &gt; 0">
						<xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum)"/>
					</xsl:when>
					<xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) &lt; 0">
						<xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum) * -1"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:variable>

			<Group 
					transType = "TR001" TransStatus="{$varTaxlotStateTx}" BuySell="" LongShort="" PosType="{$Sidevar}"
					translevel="B" ClientRef="{PBUniqueID}" Associated="{PBUniqueID}" ExecAccount="038098638" AccountID="038098638"
					ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
					SCCY="{CurrencySymbol}"	ExCode="" qty="{$QtySum}" 	Price="{format-number(AveragePrice,'###.0000')}"	type="G" prin="{format-number($GroupGrsAmt,'###.00')}" 
					comm="{format-number($CommissionSum,'###.00')}" comtype="F" 
					Taxfees="{$GroupTotalFees}" 
					Tax2="0"
					feesind="F" interest="0" interestindicator="" netamount="{format-number($GroupNetAmt,'###.00')}" hsyind="N" custbkr="{$Custodian}" mmgr="" bookid=""
					dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" comments="" fxind="" RowHeader="true"
					EntityID="{EntityID}" TaxLotState="{TaxLotState}">


				<!-- ...selecting all the records for this Group... -->
				<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
					<!-- ...and building a ThirdPartyFlatFileDetail for each -->
					<!--<xsl:if test="TaxLotState!='Deleted'">-->

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

					<xsl:variable name="PRANA_FUND_NAME" select="AccountMappedName"/>

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

					<xsl:variable name="amp">N</xsl:variable>
					
					

					<ThirdPartyFlatFileDetail
								TaxLotState="{TaxLotState}" transType = "TR001" TransStatus="{$varTaxlotState}" BuySell="" LongShort="" PosType="{$Sidevar}"
								translevel="A" ClientRef="{concat(PBUniqueID,$AccountId)}" Associated="{PBUniqueID}" ExecAccount="038098638" AccountID="{$AccountId}"
								ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
								SCCY="{CurrencySymbol}"	ExCode="" qty="{AllocatedQty}" Price="{format-number(AveragePrice,'###.0000')}" type="G" prin="{format-number($TaxlotGrsAmt,'###.00')}"
								comm="{format-number(CommissionCharged,'###.00')}" comtype="F"
								Taxfees="{$TaxFees}" 
								Tax2="0"
								feesind="F" interest="0" interestindicator="" netamount="{format-number(NetAmount,'###.00')}" hsyind="N" custbkr="{$Custodian}" mmgr="" bookid=""
								dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" comments="" fxind=""
								EntityID="{EntityID}" />
					<!--</xsl:if>-->
				</xsl:for-each>

			</Group>
		</xsl:if>




	</xsl:template>
</xsl:stylesheet>