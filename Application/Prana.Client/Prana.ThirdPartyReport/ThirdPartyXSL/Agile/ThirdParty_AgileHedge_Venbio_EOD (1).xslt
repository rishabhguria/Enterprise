<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">
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

	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />
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
		<xsl:variable name="SoftCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SoftCommissionCharged)"/>
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

		<xsl:variable name="CPVar">
			<xsl:choose>
				<xsl:when test="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
				<xsl:when test="$tempCPVar='SAND'">SDLR</xsl:when>
				<xsl:when test="$tempCPVar='ISGI'">INTS</xsl:when>
				<xsl:when test="$tempCPVar='FB'">FBCO</xsl:when>
				<xsl:when test="$tempCPVar='GS'">GSCO</xsl:when>
				<xsl:when test="$tempCPVar='MS'">MSCO</xsl:when>
				<xsl:when test="$tempCPVar='KATZ'">CANT</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="$tempCPVar"/>
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

		<xsl:variable name="varEXCODE">
			<xsl:choose>
				<xsl:when test="Symbol = 'TKMR'">
					<xsl:value-of select="'TOR'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varTaxlotStateTx">
			<xsl:choose>
				<xsl:when test="TaxLotState='Allocated'">
					<xsl:value-of select ="'NEW'"/>
				</xsl:when>
				<xsl:when test="TaxLotState='Amemded'">
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

		<!--<xsl:variable name="varCounterParty">
			<xsl:if test="CounterParty != ''">
				<xsl:value-of select="CounterParty"/>
			</xsl:if>
		</xsl:variable>

		<xsl:variable name="varExecutionBroker">
			<xsl:if test="$varCounterParty != ''">
				<xsl:value-of select="document('../ReconMappingXml/EODBrokerMapping.xml')/BrokerMapping/PB[@Name='MS']/BrokerData[@PranaBroker = $varCounterParty]/@MSBroker"/>
			</xsl:if>
		</xsl:variable>

		<xsl:variable name="varCP">
			<xsl:choose>
				<xsl:when test="$varExecutionBroker != ''">
					<xsl:value-of select ="$varExecutionBroker"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="CounterParty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

		<xsl:variable name="SecType">
			<xsl:choose>
				<xsl:when test="Asset='Equity'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'T'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varOptionUnderlying">
			<xsl:value-of select="substring-after(substring-before(Symbol,' '),':')"/>
		</xsl:variable>

		<xsl:variable name = "BlankCount_Root" >
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="(6) - string-length($varOptionUnderlying)" />
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varFormattedStrikePrice">
			<xsl:value-of select="format-number(StrikePrice,'00000.000')"/>
		</xsl:variable>

		<xsl:variable name="varOSIOptionSymbol">
			<xsl:value-of select="concat($varOptionUnderlying,$BlankCount_Root,substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2),substring(PutOrCall,1,1),translate($varFormattedStrikePrice,'.',''))"/>
		</xsl:variable>

		<xsl:variable name="varSecID">
			<xsl:choose>
				<xsl:when test ="Asset='Equity'">
					
						<xsl:value-of select="SEDOL"/>
					
				</xsl:when>
				<xsl:when test ="Asset='EquityOption'">
					
						<xsl:choose>
							<xsl:when test="OSIOptionSymbol != ''">
								<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="translate($varOSIOptionSymbol,' ','')"/>
							</xsl:otherwise>
						</xsl:choose>
					
				</xsl:when>
				<xsl:otherwise>
					
						<xsl:value-of select="Symbol"/>
					
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varFundName">
			<xsl:value-of select="FundName"/>
		</xsl:variable>

		<xsl:variable name="varPB">
			<xsl:value-of select="document('../ReconMappingXml/FundwisePBMapping.xml')/BrokerMapping/PB[@Name='MS']/BrokerData[@PranaFundName = $varFundName]/@PB"/>
		</xsl:variable>

		<xsl:variable name="varcustBkr">
			<xsl:choose>
				<xsl:when test="$varPB != ''">
					<xsl:value-of select ="$varPB"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="TotalComm" select="$CommissionSum + $SoftCommissionSum"/>


		<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

		<xsl:variable name="PB_BROKER_NAME">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='MS']/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@MLPBroker"/>
		</xsl:variable>

		<xsl:variable name="varBROKER">
			<xsl:choose>
				<xsl:when test="$PB_BROKER_NAME != ''">
					<xsl:value-of select="$PB_BROKER_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PRANA_BROKER_NAME"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<Group 
				transType = "TR001" TransStatus="{$varTaxlotStateTx}" BuySell="" LongShort="" PosType="{$Sidevar}"
				translevel="B" ClientRef="{concat(substring(EntityID,4,15),'A')}" Associated="{concat(substring(EntityID,4,15),'B')}" ExecAccount="38080099" CustAccount="38080099"
				ExecBkr="{$varBROKER}" SecType="{$SecType}" SecID="{$varSecID}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
				CCY="{CurrencySymbol}"	ExCode="" qty="{$QtySum}" 	Price="{format-number(AveragePrice,'###.0000')}"	type="G" prin="{format-number($GroupGrsAmt,'###.00')}" 
				comm="{format-number($TotalComm,'###.00')}" comtype="F" 
				Othercharges="{format-number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum,'###.00')}" 
				Taxfees="0"
				feesind="F" interest="0" interestindicator="F" netamount="{format-number($GroupNetAmt,'###.00')}" hsyind="N" custbkr="{$varcustBkr}" mmgr="" bookid=""
				dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" instx=""
				EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="">


			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>

				<!--<xsl:variable name="varTaxlotStateTx">
					<xsl:choose>
						<xsl:when test="TaxLotState='Allocated'">
							<xsl:value-of select ="'NEW'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Amemded'">
							<xsl:value-of select ="'COR'"/>
						</xsl:when>
						<xsl:when test="TaxLotState='Deleted'">
							<xsl:value-of select ="'CAN'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>-->

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

				

				<xsl:variable name="TotalCommAlloc" select="CommissionCharged + SoftCommissionCharged"/>

				<ThirdPartyFlatFileDetail
							Group_Id="" transType = "TR001" TransStatus="{$varTaxlotStateTx}" BuySell="" 
							LongShort="" PosType="{$Sidevar}" translevel="A" ClientRef="{concat(substring(EntityID,4,15),'A')}" 
							Associated="{concat(substring(EntityID,4,15),'B')}" ExecAccount="38080099" CustAccount="{FundAccountNo}"
							ExecBkr="{$varBROKER}" SecType="{$SecType}" SecID="{$varSecID}" desc="{FullSecurityName}"
							TDate="{TradeDate}" SDate="{SettlementDate}" CCY="{CurrencySymbol}"	ExCode="" 
							qty="{AllocatedQty}" Price="{format-number(AveragePrice,'###.0000')}" type="G"	
							prin="{format-number(GrossAmount,'###.00')}" comm="{format-number($TotalCommAlloc,'###.00')}" 
							comtype="F" Othercharges="{format-number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee,'###.00')}" 
							Taxfees="0"
							feesind="F" interest="0" interestindicator="F" netamount="{format-number(NetAmount,'###.00')}" hsyind="N" custbkr="{$varcustBkr}" 
							mmgr="" bookid="" dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate=""
							acqdate="" instx="" EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>
