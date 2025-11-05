<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxlotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxlotState>

				<Fund>
					<xsl:value-of select="'Fund'"/>
				</Fund>

				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>

				<Ticker>
					<xsl:value-of select="'Ticker'"/>
				</Ticker>

				<Sedol>
					<xsl:value-of select="'Sedol'"/>
				</Sedol>

				<Cusip>
					<xsl:value-of select="'Cusip'"/>
				</Cusip>

				<SecurityName>
					<xsl:value-of select="'Security Name'"/>
				</SecurityName>

				<Activity>
					<xsl:value-of select="'Activity'"/>

				</Activity>

				<Shares>
					<xsl:value-of select="'Shares'"/>
				</Shares>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select="'Settle Date'"/>
				</SettleDate>

				<LocalNetAmount>
					<xsl:value-of select="'Local Net Amount'"/>
				</LocalNetAmount>

				<LocalCommission>
					<xsl:value-of select="'Local Commission'"/>
				</LocalCommission>

				<LocalAllOtherFees>
					<xsl:value-of select="'Local All Other Fees'"/>
				</LocalAllOtherFees>

				<Cancelled>
					<xsl:value-of select="'Cancelled'"/>
				</Cancelled>

				<LongShort>
					<xsl:value-of select="'Long/Short'"/>
				</LongShort>


				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
		 <ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					 <xsl:variable name="PB_NAME" select="'USBank'"/>
					 <xsl:variable name = "PRANA_FUND_NAME">
						 <xsl:value-of select="AccountMappedName"/>
					 </xsl:variable>
					 <xsl:variable name ="THIRDPARTY_FUND_CODE">
						 <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					 </xsl:variable>
					 <Fund>
						 <xsl:choose>
							 <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								 <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							 </xsl:when>
							 <xsl:otherwise>
								 <xsl:value-of select="$PRANA_FUND_NAME"/>
							 </xsl:otherwise>
						 </xsl:choose>
					 </Fund>
					
					<ISIN>
						<xsl:choose>
							<xsl:when test="ISIN!='*'">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ISIN>

					<Ticker>
						<xsl:choose>
							<xsl:when test="Symbol!='*'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Ticker>

					<Sedol>
						<xsl:choose>
							<xsl:when test="SEDOL!='*'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Sedol>

					<Cusip>
						<xsl:choose>
							<xsl:when test="CUSIP!='*'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</Cusip>

					<SecurityName>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityName>

					<Activity>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'BY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'SL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Activity>

					<Shares>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Shares>

					<Price>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Price>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>
					 <xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>
					 <xsl:variable name="THIRDPARTY_BROKER_NAME">
						 <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_BROKER_NAME]/@PranaBroker"/>
					 </xsl:variable>
			       <Broker>
						 <xsl:choose>							
							<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
								<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>								
						 </xsl:choose>
					 </Broker>
					
					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<LocalNetAmount>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</LocalNetAmount>
					 <xsl:variable name="Commission">
						 <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					 </xsl:variable>
					<LocalCommission>
						<xsl:choose>
							<xsl:when test="number($Commission)">
								<xsl:value-of select="$Commission"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</LocalCommission>
						 <xsl:variable name="OtherFees">
							 <xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + SoftCommission"/>
						 </xsl:variable>
					<LocalAllOtherFees>
						<xsl:choose>
							<xsl:when test="number($OtherFees)">
								<xsl:value-of select="$OtherFees"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</LocalAllOtherFees>

					<Cancelled>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select ="'C'"/>
							</xsl:when>
							<!--<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'DELETE'"/>
							</xsl:when>-->
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Cancelled>

					<LongShort>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Close'">
								<xsl:value-of select="'Long'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell short'">
								<xsl:value-of select="'Short'"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</LongShort>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>