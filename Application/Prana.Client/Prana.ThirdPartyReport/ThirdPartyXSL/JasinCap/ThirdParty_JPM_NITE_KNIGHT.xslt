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

		<!--Total Quantity-->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
		</xsl:variable>

		<xsl:variable name ="varSymbol">
			<xsl:choose>
				<xsl:when test="Asset = 'EquityOption'">
					<xsl:value-of select ="OSIOptionSymbol"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varCounterParty">
			<xsl:choose>
				<xsl:when test ="CounterParty='NITE'">
						<xsl:value-of select ="'DTTX'"/>
				</xsl:when>
				<xsl:when test ="CounterParty='CSFB'">
						<xsl:value-of select ="'FBCO'"/>
				</xsl:when>
				<xsl:otherwise>
						<xsl:value-of select ="CounterParty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>



		<xsl:variable name="varCommissionCharged">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>

		<xsl:variable name="varTaxOnCommissions">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TaxOnCommissions)"/>
		</xsl:variable>

		<xsl:variable name="varStampDuty">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/StampDuty)"/>
		</xsl:variable>

		<xsl:variable name="varTransactionLevy">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TransactionLevy)"/>
		</xsl:variable>

		<xsl:variable name="varClearingFee">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ClearingFee)"/>
		</xsl:variable>
		
		<!--For reporting to KNIGHT: Do not include OTherBrokerFees and MISC FEES in the Commission data-->
		<xsl:variable name ="varCommission">
			<xsl:value-of select="$varCommissionCharged  + $varTaxOnCommissions + $varStampDuty +$varTransactionLevy +$varClearingFee"/>
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


		<Group 
			 FUND ="Compass@JPM" SIDE = "{Side}" QUANTITY="{$QtySum}" SYMBOL="{$varSymbol}" SECURITYNAME="{FullSecurityName}"
			 AVGPRICE="{AveragePrice}" COMMISSION ="{$varCommission}" ASSETNAME="{Asset}" COUNTERPARTY="{$varCounterParty}" SEDOL="{SEDOL}" 
			 EntityID="{EntityID}" TaxLotState="{TaxLotState}" RowHeader ="true" TaxLotState1="">

			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>


				<!--<ThirdPartyFlatFileDetail
					  Group_Id="" Date="{TradeDate}" AllocationAccount = "{$varAccountMapping}" B_S="{$Sidevar}" Qty="{AllocatedQty}" Symbol="{$varSymbol}"
					  Strike="{StrikePrice}" C_P="{substring(PutOrCall,1,1)}" EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>-->
			</xsl:for-each>
		</Group>
	</xsl:template>

</xsl:stylesheet>
