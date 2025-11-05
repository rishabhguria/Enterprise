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


	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />
		<xsl:variable name="AllocatedQty" />
		<!-- Building a Group with the EntityID $I_GroupID... -->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
		</xsl:variable>
		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="SoftCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SoftCommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="OtherBrokerFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OtherBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="ClearingBrokerFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ClearingBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="TaxOnCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TaxOnCommissions)"/>
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
		<xsl:variable name="SecFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SecFee)"/>
		</xsl:variable>
		<xsl:variable name="MiscFeesSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/MiscFees)"/>
		</xsl:variable>
		<xsl:variable name="OccFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OccFee)"/>
		</xsl:variable>
		<xsl:variable name="OrfFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OrfFee)"/>
		</xsl:variable>
		<xsl:variable name="GrossAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/GrossAmount)"/>
		</xsl:variable>
		<xsl:variable name="NetAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
		</xsl:variable>	
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
		</xsl:variable>
		<xsl:variable name="tempCPVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CounterParty"/>
		</xsl:variable>				<xsl:variable name="Pb_Name" select="'Banco Popular'"/>

		<xsl:variable name="CP">			
			<xsl:value-of select="$tempCPVar"/>			
		</xsl:variable>

		<xsl:variable name="Side">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$tempSideVar='BuSell short' or $tempSideVar='Sell to Open'">
					<xsl:value-of select="'SS'"/>
				</xsl:when>
				<xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">
					<xsl:value-of select="'BC'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="Symbol">
			<xsl:value-of select="concat(Symbol,' Equity')"/>
		</xsl:variable>

		<xsl:variable name="varAveragePrice">
			<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
		</xsl:variable>

		<xsl:variable name="varGrossAmount">
			<xsl:value-of select="$QtySum*$varAveragePrice*AssetMultiplier"/>
		</xsl:variable>

		<xsl:variable name="varOtherCharges">
			<xsl:value-of select="$CommissionSum + $SoftCommissionSum + $OtherBrokerFeeSum + $ClearingBrokerFeeSum + $TaxOnCommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $SecFeeSum + $MiscFeesSum + $OccFeeSum + $OrfFeeSum"/>
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
			<xsl:value-of select="$varGrossAmount + ($varOtherCharges * $varSideMul)"/>
		</xsl:variable>
		

		<xsl:variable name="AveragePrice">
			<xsl:choose>
				<xsl:when test="AveragePrice &gt; 0">
					<xsl:value-of select="AveragePrice"/>
				</xsl:when>
				<xsl:when test="AveragePrice &lt; 0">
					<xsl:value-of select="AveragePrice * (-1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'-'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GrossAmountSumG">
			<xsl:choose>
				<xsl:when test="$varGrossAmount &gt; 0">
					<xsl:value-of select="$varGrossAmount"/>
				</xsl:when>
				<xsl:when test="$varGrossAmount &lt; 0">
					<xsl:value-of select="$varGrossAmount * (-1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'-'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="CommissionSumG">
			<xsl:choose>
				<xsl:when test="$CommissionSum &gt; 0">
					<xsl:value-of select="$CommissionSum"/>
				</xsl:when>
				<xsl:when test="$CommissionSum &lt; 0">
					<xsl:value-of select="$CommissionSum * (-1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'-'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="StampDutySumG">
			<xsl:choose>
				<xsl:when test="$StampDutySum &gt; 0">
					<xsl:value-of select="$StampDutySum"/>
				</xsl:when>
				<xsl:when test="$StampDutySum &lt; 0">
					<xsl:value-of select="$StampDutySum * (-1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'-'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="SecFeeSumG">
			<xsl:choose>
				<xsl:when test="$SecFeeSum &gt; 0">
					<xsl:value-of select="$SecFeeSum"/>
				</xsl:when>
				<xsl:when test="$SecFeeSum &lt; 0">
					<xsl:value-of select="$SecFeeSum * (-1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'-'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="NetAmountSumG">
			<xsl:choose>
				<xsl:when test="$varNetAmmount &gt; 0">
					<xsl:value-of select="$varNetAmmount"/>
				</xsl:when>
				<xsl:when test="$varNetAmmount &lt; 0">
					<xsl:value-of select="$varNetAmmount * (-1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'-'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name = "PRANA_FUND_NAME">
			<xsl:value-of select="AccountName"/>
		</xsl:variable>

		<xsl:variable name ="THIRDPARTY_FUND_CODE">
			<xsl:value-of select ="document('../ReconMappingXml/MasterFundMapping.xml')/MasterFundMapping/PB[@Name=$Pb_Name]/MasterFundData[@FundName=$PRANA_FUND_NAME]/@MasterFundName"/>
		</xsl:variable>


		<xsl:variable name="Account">
			<xsl:choose>
				<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
					<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PRANA_FUND_NAME"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="CL_Broker" select="'USB'"/>
		<xsl:variable name ="THIRDPARTY_BROKER1">
			<xsl:value-of select ="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$CL_Broker]/BrokerData[ @PranaBroker=$tempCPVar]/@ThirdPartyBrokerID"/>
		</xsl:variable>

		<xsl:variable name="AccountId">
			<xsl:choose>

				<xsl:when test="$THIRDPARTY_BROKER1!=''">
					<xsl:value-of select="$THIRDPARTY_BROKER1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<Group
		  RowHeader="true"  Account="451029300" Tradedate="{TradeDate}" Settledate="{SettlementDate}" Name="{FullSecurityName}" Ticker="{$Symbol}"
		  Cusip="{CUSIP}" Side="{$Side}" Shares="{$QtySum}"
		  Tradeprice="{concat('$',format-number($AveragePrice,'0.####'))}" Principal="{concat('$',format-number($GrossAmountSumG,'0.##'))}" Commission="{concat('$',$CommissionSumG)}" SECfee="{concat('$',$SecFeeSumG)}"
		  Netmoney="{concat('$',format-number($NetAmountSumG,'0.##'))}" Broker="{$CP}" DTC="{$AccountId}"
		  EntityID="{EntityID}" TaxLotState="{TaxLotState}">


			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>

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
							<xsl:value-of select ="'NEW'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="varAveragePriceTaxlot">
					<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
				</xsl:variable>

				<xsl:variable name="varGrossAmountTaxlot">
					<xsl:value-of select="AllocatedQty*$varAveragePriceTaxlot*AssetMultiplier"/>
				</xsl:variable>

				<xsl:variable name="varOtherChargesTaxlot">
					<xsl:value-of select="CommissionCharged + SoftCommissionCharged + OtherBrokerFee + ClearingBrokerFee + TaxOnCommissions + StampDuty + TransactionLevy + ClearingFee + SecFee + MiscFees + OccFee + OrfFee"/>
				</xsl:variable>
				

				<xsl:variable name="varNetAmmountTaxlot">
					<xsl:value-of select="$varGrossAmountTaxlot + ($varOtherChargesTaxlot * $varSideMul)"/>
				</xsl:variable>
				
				<xsl:variable name="GrossAmount">
					<xsl:choose>
						<xsl:when test="$varGrossAmountTaxlot &gt; 0">
							<xsl:value-of select="$varGrossAmountTaxlot"/>
						</xsl:when>
						<xsl:when test="$varGrossAmountTaxlot &lt; 0">
							<xsl:value-of select="$varGrossAmountTaxlot * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'-'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="CommissionCharged">
					<xsl:choose>
						<xsl:when test="CommissionCharged &gt; 0">
							<xsl:value-of select="CommissionCharged"/>
						</xsl:when>
						<xsl:when test="CommissionCharged &lt; 0">
							<xsl:value-of select="CommissionCharged * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'-'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="Secfees">
					<xsl:choose>
						<xsl:when test="SecFee &gt; 0">
							<xsl:value-of select="SecFee"/>
						</xsl:when>
						<xsl:when test="SecFee &lt; 0">
							<xsl:value-of select="SecFee * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'-'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="StampDuty">
					<xsl:choose>
						<xsl:when test="StampDuty &gt; 0">
							<xsl:value-of select="StampDuty"/>
						</xsl:when>
						<xsl:when test="StampDuty &lt; 0">
							<xsl:value-of select="StampDuty * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'-'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="NetAmount">
					<xsl:choose>
						<xsl:when test="$varNetAmmountTaxlot &gt; 0">
							<xsl:value-of select="$varNetAmmountTaxlot"/>
						</xsl:when>
						<xsl:when test="$varNetAmmountTaxlot &lt; 0">
							<xsl:value-of select="$varNetAmmountTaxlot * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'-'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<ThirdPartyFlatFileDetail
				  RowHeader="true" TaxLotState="{TaxLotState}" Account="451029300" Tradedate="{TradeDate}" Settledate="{SettlementDate}" Name="{FullSecurityName}" Ticker="{$Symbol}"
				  Cusip="{CUSIP}" Side="{$Side}" Shares="{AllocatedQty}"
				  Tradeprice="{concat('$',format-number($AveragePrice,'0.####'))}" Principal="{concat('$',format-number($GrossAmount,'0.##'))}" Commission="{concat('$',$CommissionCharged)}" SECfee="{concat('$',$Secfees)}"
				  Netmoney="{concat('$',format-number($NetAmount, '0.##'))}" Broker="{$CP}" DTC="{$AccountId}"
				  EntityID="{EntityID}" />
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>
