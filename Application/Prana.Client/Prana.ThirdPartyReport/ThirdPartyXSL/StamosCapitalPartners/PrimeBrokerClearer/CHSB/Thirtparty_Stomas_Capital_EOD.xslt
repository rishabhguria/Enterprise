<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>

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


			<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

			<xsl:variable name="THIRDPARTY_FUND_NAME">
				<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
			</xsl:variable>

			<xsl:variable name="varAccountId">
				<xsl:choose>
					<xsl:when test="$THIRDPARTY_FUND_NAME!=''">
						<xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PRANA_FUND_NAME"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varAction">
				<xsl:choose>
					
					<xsl:when test="$tempSideVar = 'Sell' and Asset='Equity'">
						<xsl:value-of select="'S'"/>
					</xsl:when>
					<xsl:when test="$tempSideVar='Buy'  and Asset='Equity'">
						<xsl:value-of select="'B'"/>
					</xsl:when>

					<xsl:when test="$tempSideVar = 'Sell' and Asset='FixedIncome'">
						<xsl:value-of select="'S'"/>
					</xsl:when>
					<xsl:when test="$tempSideVar='Buy'  and Asset='FixedIncome'">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					
					<xsl:when test="Side = 'Sell short' and Asset='Equity'">
						<xsl:value-of select="'SS'"/>
					</xsl:when>

					<xsl:when test="$tempSideVar = 'Sell' and Asset='PrivateEquity'">
						<xsl:value-of select="'S'"/>
					</xsl:when>
					<xsl:when test="$tempSideVar='Buy'  and Asset='PrivateEquity'">
						<xsl:value-of select="'B'"/>
					</xsl:when>

					<xsl:when test="Side = 'Sell short' and Asset='PrivateEquity'">
						<xsl:value-of select="'SS'"/>
					</xsl:when>
					
					<xsl:when test="Side = 'Buy to Open' and Asset = 'EquityOption'">
						<xsl:value-of select="'BO'"/>
					</xsl:when>
					<xsl:when test="Side = 'Buy to Close' and Asset = 'EquityOption'">
						<xsl:value-of select="'BC'"/>
					</xsl:when>
					<xsl:when test="Side = 'Sell to Open' and Asset = 'EquityOption'">
						<xsl:value-of select="'SO'"/>
					</xsl:when>
					<xsl:when test="Side = 'Sell to Close' and Asset='EquityOption'">
						<xsl:value-of select="'SC'"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>



			<xsl:variable name="varInstruction">
				<xsl:choose>
					<xsl:when test="TaxLotState = 'Allocated'">
						<xsl:value-of select="'NEW'"/>
					</xsl:when>
					<xsl:when test="TaxLotState = 'Amemded'">
						<xsl:value-of select="'MOD'"/>
					</xsl:when>
					<xsl:when test="TaxLotState = 'Deleted'">
						<xsl:value-of select="'CXL'"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varSymbol">
				<xsl:choose>
					<xsl:when test="Asset = 'Equity'">
						<xsl:value-of select="Symbol"/>
					</xsl:when>
					<xsl:when test="Asset = 'PrivateEquity'">
						<xsl:value-of select="Symbol"/>
					</xsl:when>
					<xsl:when test="Asset = 'FixedIncome'">
						<xsl:value-of select="Symbol"/>
					</xsl:when>
					<xsl:when test ="Asset='EquityOption'">
						<xsl:value-of select="OSIOptionSymbol"/>
					</xsl:when>				
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varSecurityType">
				<xsl:choose>
					<xsl:when test="Asset = 'EquityOption'">
						<xsl:value-of select="'OPTION'"/>
					</xsl:when>
					<xsl:when test="Asset = 'PrivateEquity'">
						<xsl:value-of select="'MF'"/>
					</xsl:when>
					<xsl:when test="Asset = 'FixedIncome'">
						<xsl:value-of select="'Bond'"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity'">
						<xsl:value-of select="'EQUITY'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varQuantity">

				<xsl:choose>
					<xsl:when test="Asset = 'EquityOption'">
						<xsl:value-of select="format-number(AllocatedQty,'#####.##')"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity' and Side='Buy'">
						<xsl:value-of select="format-number(AllocatedQty,'######.##')"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity' and (Side='Sell' or Side='Sell short')">
						<xsl:value-of select="format-number(AllocatedQty,'######.####')"/>
					</xsl:when>
					<xsl:when test="Asset = 'PrivateEquity' and Side='Buy'">
						<xsl:value-of select="format-number(AllocatedQty,'######.##')"/>
					</xsl:when>
					<xsl:when test="Asset = 'PrivateEquity' and (Side='Sell' or Side='Sell short')">
						<xsl:value-of select="format-number(AllocatedQty,'######.####')"/>
					</xsl:when>
				
					<xsl:when test="Asset = 'FixedIncome' and Side='Buy'">
						<xsl:value-of select="format-number(AllocatedQty,'######.##')"/>
					</xsl:when>
					<xsl:when test="Asset = 'FixedIncome' and (Side='Sell' or Side='Sell short')">
						<xsl:value-of select="format-number(AllocatedQty,'######.####')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>


			<xsl:variable name="varLimitPrice">

				<xsl:choose>
					<xsl:when test="Asset = 'EquityOption'">
						<xsl:value-of select="format-number(AllocatedQty,'#####.####')"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity' ">
						<xsl:value-of select="format-number(AllocatedQty,'######.####')"/>
					</xsl:when>
					
				
				</xsl:choose>
			</xsl:variable>


			<xsl:variable name="varOrderType">
				<xsl:choose>
					<xsl:when test="OrderType='Market'">
						<xsl:value-of select="MKT"/>
					</xsl:when>
					<xsl:when test="OrderType='Limit'">
						<xsl:value-of select="LMT"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varLimitType">
				<xsl:choose>
					<xsl:when test="Asset = 'EquityOption'">
						<xsl:value-of select="format-number(AllocatedQty,'#####.####')"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity'">
						<xsl:value-of select="format-number(AllocatedQty,'######.####7')"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity' and (Side='Sell' or Side='Sell short')">
						<xsl:value-of select="format-number(AllocatedQty,'######.####')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="varLimitTime">
				<xsl:choose>
					<xsl:when test="Asset = 'EquityOption'">
						<xsl:value-of select="format-number(AllocatedQty,'#####.####')"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity'">
						<xsl:value-of select="format-number(AllocatedQty,'######.####7')"/>
					</xsl:when>
					<xsl:when test="Asset = 'Equity' and (Side='Sell' or Side='Sell Short')">
						<xsl:value-of select="format-number(AllocatedQty,'######.####')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>


			<!--<xsl:variable name ="varStopPrice">

			<xsl:choose>
				<xsl:when test="OrderType='Market'">
					<xsl:value-of select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test ="contains(Asset, 'Option')!= false">
							<xsl:value-of select ="format-number(StopPrice,'#####.####)'"/>
						</xsl:when>
						<xsl:when test ="Asset ='Equity'">
							<xsl:value-of select ="format-number(StopPrice,'#####.####)'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>	
			</xsl:variable>-->






			<Group
					 AccountNumber="{$varAccountId}"  Action="{$varAction}"  Quantity="{$varQuantity}" SecuritySymbol="{$varSymbol}" Timing="" LimitPrice="{$varLimitPrice}"
				  TimeLimit="" SecurityType="{$varSecurityType}" StopPrice="" AllorNone="" MinimumQuantity="{OrderQty}" DoNotReduce=""
					ReinvestDividends = "" TransactionFee = "" SwaptoFund="" DoNotSubmit=""
					 LinkedTradeID="" NewMoney="" Executingbroker="{$CPVar}" InitiatorId="" Discretionary=""
					  TraderId="{PBUniqueID}" OlCustomColumn1="" OlCustomColumn2="" OlCustomColumn3="" OlCustomColumn4="" OlCustomColumn5=""
					 EtfType="" OrderDate="{TradeDate}" LotSelectionMethod="VPS" RecordType="EV"
					 PurchaseDate="{OriginalPurchaseDate}"  ShareQuantity="" PurchasePrice=""
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

					<xsl:variable name="amp">N</xsl:variable>

					<ThirdPartyFlatFileDetail
								TaxLotState="{TaxLotState}"  AccountNumber="{$varAccountId}"  Action="{$varAction}"  Quantity="{$varQuantity}" SecuritySymbol="{$varSymbol}" Timing="" LimitPrice=""
				  TimeLimit="" SecurityType="{$varSecurityType}" StopPrice="" AllorNone="" MinimumQuantity="{OrderQty}" DoNotReduce=""
					ReinvestDividends = "" TransactionFee = "" SwaptoFund="" DoNotSubmit=""
					 LinkedTradeID="" NewMoney="" Executingbroker="{$CPVar}" InitiatorId="" Discretionary=""
					  TraderId="{PBUniqueID}" OlCustomColumn1="" OlCustomColumn2="" OlCustomColumn3="" OlCustomColumn4="" OlCustomColumn5=""
					 EtfType="" OrderDate="{TradeDate}" LotSelectionMethod="VPS" RecordType="EV"
					 PurchaseDate="{OriginalPurchaseDate}"  ShareQuantity="" PurchasePrice="" 
						EntityID="{EntityID}"/>
						<!--</xsl:if>-->
					</xsl:for-each>

			</Group>
		</xsl:if>

	</xsl:template>
</xsl:stylesheet>