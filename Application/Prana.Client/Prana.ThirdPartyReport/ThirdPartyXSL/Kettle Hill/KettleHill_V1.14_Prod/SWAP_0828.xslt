<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<!-- looping for generating blank file with header             -->
			
			<Group 
				transType = "transType" TransStatus="TransStatus" BuySell="BuySell" LongShort="LongShort" PosType="PosType"
				translevel="translevel" ClientRef="ClientRef" Associated="Associated" ExecAccount="ExecAccount" AccountID="AccountID"
				ExecBkr="ExecBkr" SecType="SecType" SecID="SecID" desc="desc" TDate="TDate" SDate="SDate"
				CCY="CCY"	ExCode="ExCode" qty="qty" 	Price="Price"	type="type" prin="prin" 
				comm="comm" comtype="comtype" msfees="msfees" IgnoredField="IgnoredField" msfeesind="msfeesind"
				Divpercent="Divpercent"	spread="spread" netamount="netamount" hsyind="hsyind" custbkr="custbkr" mmgr="mmgr" bookid="bookid"
				dealid="dealid" taxlotid="taxlotid" taxdate="taxdate" taxprice="taxprice" closeoutmethod="closeoutmethod" exrate="exrate" acqdate="acqdate" comments="comments"
				swafrefno="swafrefno" basketid="basketid"   pricecur="pricecur"  resetind="resetind" SNSreference="SNSreference" ResearchFee="ResearchFee"   ResearchFeeIndicator="ResearchFeeIndicator"  LEI="LEI" 
				EntityID="" TaxLotState=""  TaxLotState1="" IsCaptionChangeRequired="true" RowHeader="false"/>


			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[AccountName ='Heptagon - Shorts - Morgan Stanley']">
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

	<xsl:template name="Conversion">
		<xsl:param name="Value"/>
		<xsl:param name="Curr"/>

		<xsl:choose>
			<xsl:when test="Asset='Equity' and IsSwapped='true'">
				<xsl:choose>
					<xsl:when test="FXConversionMethodOperator_Taxlot='M'">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>
								
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value div FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select="$Value"/>
			</xsl:otherwise>
		</xsl:choose>

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
		<xsl:variable name="SoftCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SoftCommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="BrokerCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OtherBrokerFee)"/>
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
		<!--<xsl:variable name="NetAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
		</xsl:variable>-->
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

		<xsl:variable name="Sidevar">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">BL</xsl:when>
				<xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">BC</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">SL</xsl:when>
				<xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">SS</xsl:when>
				<xsl:otherwise> </xsl:otherwise>
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


		

		<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<xsl:value-of select="Symbol"/>
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
				<xsl:when test ="Bloomberg!=''">
					<xsl:value-of select="Bloomberg"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
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
				
				<xsl:when test ="Bloomberg!=''">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupGrsAmt">
			<xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
		</xsl:variable>

		<xsl:variable name="GroupSideMultiplier">
			<xsl:choose>
				<xsl:when test="contains($tempSideVar,'Buy')">
					<xsl:value-of select="1"/>
				</xsl:when>
				<xsl:when test="contains($tempSideVar,'Sell')">
					<xsl:value-of select="-1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupNetAmt1">
			
				
					<xsl:value-of select="$GroupNetAmt"/>
				
		
		</xsl:variable>

		<xsl:variable name="CurrencySymbol">


			<xsl:value-of select="CurrencySymbol"/>


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

		

		<xsl:variable name="Price_Gp">
			<xsl:call-template name="Conversion">
				<xsl:with-param name="Value" select="AveragePrice"/>
				<xsl:with-param name="Curr" select="CurrencySymbol"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="Comm_Gp">
			<xsl:call-template name="Conversion">
				<xsl:with-param name="Value" select="$CommissionSum"/>
				<xsl:with-param name="Curr" select="CurrencySymbol"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="Principal_Gp">		
			<xsl:call-template name="Conversion">
				<xsl:with-param name="Value" select="$GroupGrsAmt"/>
				<xsl:with-param name="Curr" select="CurrencySymbol"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="Netamount_Gp">		
			<xsl:call-template name="Conversion">
				<xsl:with-param name="Value" select="$GroupNetAmt"/>
				<xsl:with-param name="Curr" select="CurrencySymbol"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="MsFee_Gp">		

			<xsl:call-template name="Conversion">
				<xsl:with-param name="Value" select="(OtherBrokerFee  + StampDuty + TaxOnCommissions + MiscFees + OrfFee)"/>
				<xsl:with-param name="Curr" select="CurrencySymbol"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="COMM2" select="format-number(CommissionCharged + SoftCommissionCharged,'###.00')"/>

		<xsl:variable name="COMM">
		
				
					<xsl:value-of select="$COMM2"/>
			
		</xsl:variable>
		<xsl:variable name="AveragePrice">
			
					<xsl:value-of select="AveragePrice"/>
			
		</xsl:variable>

		<xsl:variable name="GrossAmnt">
		
					<xsl:value-of select="$GroupGrsAmt"/>
				
		</xsl:variable>
		
		<xsl:variable name="FxRate">
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
		<xsl:variable name="TaxlotGRS" select="AllocatedQty * format-number(AveragePrice,'###.000000') * AssetMultiplier"/>
		<xsl:variable name="TaxlotGrsAmt">
			
					<xsl:value-of select="$TaxlotGRS"/>
				
		</xsl:variable>

		<xsl:variable name="NTANT1" select="format-number(NetAmount,'###.00')"/>
		<xsl:variable name="AMNT1">
		
				
					<xsl:value-of select="$NTANT1"/>
				
		</xsl:variable>

		<Group 
				transType = "SW002" TransStatus="{$varTaxlotStateTx}" BuySell="" LongShort="" PosType="{$Sidevar}"
				translevel="B" ClientRef="{PBUniqueID}" Associated="{PBUniqueID}" ExecAccount="038082566" AccountID="038082566"
				ExecBkr="{$CPVar}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
				CCY="{$CurrencySymbol}"	ExCode="" qty="{AllocatedQty}" 	Price="{format-number($AveragePrice,'#.0000000')}"	type="G" prin="{format-number($TaxlotGrsAmt,'#.0000000')}" 
				comm="{format-number($COMM,'#.0000000')}" comtype="F" msfees="{format-number($MsFee_Gp,'0.##')}" IgnoredField="" msfeesind="F"
				Divpercent=""	spread="" netamount="{format-number($AMNT1,'#.0000000')}" hsyind="N" custbkr="MSCO" mmgr="" bookid=""
				dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="1" acqdate="" comments=""
				swafrefno="" basketid=""   pricecur=""  resetind="" 
				EntityID="{EntityID}" TaxLotState="{TaxLotState}"  TaxLotState1="" IsCaptionChangeRequired="true" RowHeader="false">


			<!-- ...selecting all the records for this Group...and AccountName='UCITS - Shorts - Morgan Stanley' -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and AccountName='Heptagon - Shorts - Morgan Stanley']">
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

				<!--<xsl:variable name="TaxlotGrsAmt">
					<xsl:value-of select="AllocatedQty * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
				</xsl:variable>-->
				

				<xsl:variable name="TaxlotSideMultiplier">
					<xsl:choose>
						<xsl:when test="contains($tempSideVar,'Buy')">
							<xsl:value-of select="1"/>
						</xsl:when>
						<xsl:when test="contains($tempSideVar,'Sell')">
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

				<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

				<xsl:variable name="PB_COUNTERPARTY_NAME_A">
					<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
				</xsl:variable>

				<xsl:variable name="Cpty">
					<xsl:choose>
						<xsl:when test="$PB_COUNTERPARTY_NAME_A!=''">
							<xsl:value-of select="$PB_COUNTERPARTY_NAME_A"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="CounterParty"/>
						</xsl:otherwise>
					</xsl:choose>
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

				<xsl:variable name="Price_Al">					

					<xsl:call-template name="Conversion">
						<xsl:with-param name="Value" select="AveragePrice"/>
						<xsl:with-param name="Curr" select="CurrencySymbol"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="Comm_Al">					

					<xsl:call-template name="Conversion">
						<xsl:with-param name="Value" select="CommissionCharged"/>
						<xsl:with-param name="Curr" select="CurrencySymbol"/>
					</xsl:call-template>		

				</xsl:variable>

				<xsl:variable name="Principal_Al">				

					<xsl:call-template name="Conversion">
						<xsl:with-param name="Value" select="$TaxlotGrsAmt"/>
						<xsl:with-param name="Curr" select="CurrencySymbol"/>
					</xsl:call-template>

				</xsl:variable>

				<xsl:variable name="Netamount_Al">					

					<xsl:call-template name="Conversion">
						<xsl:with-param name="Value" select="NetAmount"/>
						<xsl:with-param name="Curr" select="CurrencySymbol"/>
					</xsl:call-template>

				</xsl:variable>
				

				<xsl:variable name="MsFee_Al">				
					<xsl:call-template name="Conversion">
						<xsl:with-param name="Value" select="(OtherBrokerFee  + StampDuty + TaxOnCommissions + MiscFees + OrfFee)"/>
						<xsl:with-param name="Curr" select="CurrencySymbol"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="NTANT" select="format-number(NetAmount,'###.00')"/>
				<xsl:variable name="AMNT">
					
						
							<xsl:value-of select="$NTANT"/>
						
				</xsl:variable>
				
				<xsl:variable name="COMM1" select="format-number(CommissionCharged + SoftCommissionCharged,'###.00')"/>
				<xsl:variable name="Commission1">
				
							<xsl:value-of select="$COMM1"/>
						
				</xsl:variable>

				<xsl:variable name="FxRate_Al">
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

				<ThirdPartyFlatFileDetail
							Group_Id=""  transType = "SW002" TransStatus="{$varTaxlotState}" BuySell="" 
							LongShort="" PosType="{$Sidevar}" translevel="A" ClientRef="{concat(substring(EntityID,3),'C')}" 
							Associated="{PBUniqueID}" ExecAccount="038082566" AccountID="{AccountMappedName}"
							ExecBkr="{$Cpty}" SecType="{$varSecType}" SecID="{$varSymbol}" desc="{FullSecurityName}"
							TDate="{TradeDate}" SDate="{SettlementDate}" CCY="{$CurrencySymbol}"	ExCode="" 
							qty="{AllocatedQty}" Price="{format-number($AveragePrice,'#.0000000')}" type="G"	
							prin="{format-number($TaxlotGrsAmt,'#.0000000')}" comm="{format-number($Commission1,'#.0000000')}" 
							comtype="F" msfees="{format-number($MsFee_Al,'0.##')}" IgnoredField="" msfeesind="F"
							Divpercent=""	spread="" netamount="{format-number($AMNT,'#.0000000')}" hsyind="N" custbkr="MSCO" 
							mmgr="" bookid="" dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="1"
							acqdate="" comments="" swafrefno="" basketid=""   pricecur=""  resetind="" SNSreference="" ResearchFee=""   ResearchFeeIndicator=""  LEI="" 
							EntityID="{EntityID}" TaxLotState="{TaxLotState}" IsCaptionChangeRequired="true" RowHeader="false"/>
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>