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

		<xsl:variable name ="varTransType">
			<xsl:choose>
				<xsl:when test="Side='Buy to Open' or Side='Buy' ">
						<xsl:value-of select ="'BY'"/>
				</xsl:when>
				<xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
						<xsl:value-of select ="'CS'"/>
				</xsl:when>
				<xsl:when test="Side='Sell' or Side='Sell to Close' ">
						<xsl:value-of select ="'SL'"/>
				</xsl:when>
				<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
						<xsl:value-of select ="'SS'"/>
				</xsl:when>
				<xsl:otherwise>
						<xsl:value-of select="Side"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varCheckSymbolUnderlying">
			<xsl:value-of select ="substring-before(Symbol,'-')"/>
		</xsl:variable>

		<xsl:variable name ="varInstrumentId">
			
			<xsl:choose>
				<xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:when test ="Asset ='EquityOption' ">
					<xsl:value-of select="concat('Q',translate(Symbol,' ',''))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExecutingBroker">
			<xsl:choose>
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

		<xsl:variable name ="varCommission">
			<xsl:value-of select="$varCommissionCharged  + $varTaxOnCommissions"/>
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
			 TradeId="{TradeRefID}" Moniker = "P0336" TransactionType="{$varTransType}" Quantity="{$QtySum}" InstrumentId="{$varInstrumentId}"
			 Price="{AveragePrice}" AccountId="43200274" ExecutingBroker ="{$varExecutingBroker}" TradeDate ="{TradeDate}" SettleDate="" CommissionType="T"
			 Commission ="{$varCommission}" SellingMethod="" Vs_purchases_Date="" SettlementCurrency="" SettlementExchangeRate="" Exchange=""
			 OtherFee ="0" Strategy="" LotNumber="0" LotQuantity="0" Trader="" Interest="0" Custodian="" WhenIssued="N" 
			 EntityID="{EntityID}" TaxLotState="{TaxLotState}" RowHeader ="false" TaxLotState1="">

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

				<xsl:variable name="varAccountMapping">
					<xsl:choose>
						<xsl:when test="FundName = 'SSARIS'">
							<xsl:value-of select="'05A950171'"/>
						</xsl:when>
						<xsl:when test="FundName = 'Master Fund'">
							<xsl:value-of select="'70MA50'"/>
						</xsl:when>
						<xsl:when test="FundName = 'DB Select'">
							<xsl:value-of select="'L16106'"/>
						</xsl:when>
						<xsl:when test="FundName = 'NewFinance'">
							<xsl:value-of select="'70MA50'"/>
						</xsl:when>
						<xsl:when test="FundName = 'Tracker'">
							<xsl:value-of select="'A16484'"/>
						</xsl:when>
						<xsl:when test="FundName = 'TV Notional'">
							<xsl:value-of select="'A16485'"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select ="FundName"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<!--<ThirdPartyFlatFileDetail
					  Group_Id="" Date="{TradeDate}" AllocationAccount = "{$varAccountMapping}" B_S="{$Sidevar}" Qty="{AllocatedQty}" Symbol="{$varSymbol}"
					  Strike="{StrikePrice}" C_P="{substring(PutOrCall,1,1)}" EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>-->
			</xsl:for-each>
		</Group>
	</xsl:template>
	
</xsl:stylesheet>
