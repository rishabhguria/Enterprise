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

		<!--Total Quantity-->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
		</xsl:variable>

		<!-- They need it blank -->
		<!--<xsl:variable name="QtySum">
			<xsl:value-of  select="''"/>
		</xsl:variable>-->
		<!--Total Commission-->
		<xsl:variable name="VarCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
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

		<xsl:variable name="varTradeDate">
			<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
		</xsl:variable>

		<xsl:variable name="varSettlementDate">
			<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
		</xsl:variable>

		<xsl:variable name="varSide">
			<xsl:value-of select="substring(Side,1,1)"/>
		</xsl:variable>



		<Group 
					RowHeader="false" Record_Identifier ="OR" Block_Order_Id ="{PBUniqueID}" Client_Identifier ="Lyrical" TradeDate ="{$varTradeDate}" 
					SettlementDate ="{$varSettlementDate}" Side ="{$varSide}" SecurityID_Type = "A" SecurityID = "{Symbol}" Suffix=""  
					Shares="{$QtySum}" AveragePrice="{AveragePrice}" Solicited_UnSolicited_Code="U" 
					CommissionType="4" CommissionAmount="0" Step_IN_Indicator="" Open_Close_Indicator="" Covered_Uncovered_indicator="" 				  
					EntityID="{EntityID}" TaxLotState="{TaxLotState}" FileHeader="true" FileFooter="true">

			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>

				<xsl:variable name="var1">
					<xsl:value-of select="position()"/>
				</xsl:variable>


				<ThirdPartyFlatFileDetail
				
					RowHeader="false" TaxLotState="{TaxLotState}" Record_Identifier ="AR" Block_Order_Id ="{PBUniqueID}" Allocation_Id="{EntityID}"  AccountNumber="{AccountNo}" Account_Type="0" AccountRR="" 
					   Shares="{AllocatedQty}" CommissionType="4" CommissionAmount="0" Versus_PurchaseDate="" StepOutIndicator=""/>
				
				<!--<ThirdPartyFlatFileDetail
				
					 RowHeader="false"  TaxLotState="{TaxLotState}" Record_Identifier ="AR" Block_Order_Id ="{PBUniqueID}" Allocation_Id="{EntityID}"  AccountNumber="{FundAccountNo}" Account_Type="0" AccountRR="" 
					   Shares="{AllocatedQty}" CommissionType="4" CommissionAmount="0" Versus_PurchaseDate="" StepOutIndicator="" StepoutBroker="" 
					   Filler1=""	filler2="" filler3="" filler4="" filler5=""    
					  EntityID="{EntityID}" FileHeader="TRUE" FileFooter="TRUE" />-->
				
					   
				<!--EntityID="{EntityID}" TaxLotState="{TaxLotState}"  FileHeader="TRUE" FileFooter="TRUE"-->
				
				<!--RowHeader="false"  -->
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>
