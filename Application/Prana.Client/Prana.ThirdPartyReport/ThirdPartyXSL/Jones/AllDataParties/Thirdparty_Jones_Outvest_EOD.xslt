<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>


				<PortfolioName>
					<xsl:value-of select="'Portfolio Name'"/>
				</PortfolioName>

				<SecurityID>
					<xsl:value-of select="'Security ID'"/>
				</SecurityID>

				<Quantity>
					<xsl:value-of select ="'Quantity'"/>
				</Quantity>

				<TransactionPrice>
					<xsl:value-of select ="'Transaction Price'"/>
				</TransactionPrice>

				<Date>
					<xsl:value-of select="'Date'"/>
				</Date>


				<TransactionType>
					<xsl:value-of select="'Transaction Type'"/>
				</TransactionType>


				<TransactionTypeCode>
					<xsl:value-of select="'Transaction Type Code'"/>
				</TransactionTypeCode>



				<Description>
					<xsl:value-of select="'Description'"/>
				</Description>



				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<xsl:variable name="PB_NAME" select="'Jones'"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>


					<xsl:variable name="varAccountName">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PortfolioName>
						<xsl:value-of select="$varAccountName"/>
					</PortfolioName>

					<SecurityID>
						<xsl:choose>
							<xsl:when test ="BBCode!=''">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test ="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test ="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>

					<Quantity>
						<xsl:value-of select ="AllocatedQty"/>
					</Quantity>

					<TransactionPrice>
						<xsl:value-of select ="AveragePrice"/>
					</TransactionPrice>

					<Date>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</Date>

					<xsl:choose>
						<xsl:when test="Side ='Buy' or Side='Buy to Open'">
							<TransactionType>
								<xsl:value-of select="'Buy Long'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side ='Buy to Close'">
							<TransactionType>
								<xsl:value-of select="'But To Cover'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side='Sell short'or Side='Sell to Open'">
							<TransactionType>
								<xsl:value-of select="'Sell Short'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<TransactionType>
								<xsl:value-of select="'Sell Long'"/>
							</TransactionType>
						</xsl:when>
						<xsl:otherwise>
							<TransactionType>
								<xsl:value-of select="''"/>
							</TransactionType>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test="Side ='Buy' or Side='Buy to Open'">
							<TransactionTypeCode>
								<xsl:value-of select="'BY'"/>
							</TransactionTypeCode>
						</xsl:when>
						<xsl:when test="Side ='Buy to Close'">
							<TransactionTypeCode>
								<xsl:value-of select="'BC'"/>
							</TransactionTypeCode>
						</xsl:when>
						<xsl:when test="Side='Sell short'or Side='Sell to Open'">
							<TransactionTypeCode>
								<xsl:value-of select="'SS'"/>
							</TransactionTypeCode>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<TransactionTypeCode>
								<xsl:value-of select="'SL'"/>
							</TransactionTypeCode>
						</xsl:when>
						<xsl:otherwise>
							<TransactionTypeCode>
								<xsl:value-of select="''"/>
							</TransactionTypeCode>
						</xsl:otherwise>
					</xsl:choose>


					<Description>
						<xsl:value-of select="''"/>
					</Description>



					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
