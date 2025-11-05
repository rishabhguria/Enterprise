<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!-- this field use internal purpose-->

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<FUNDNAME>
						<xsl:value-of select="FundName"/>
					</FUNDNAME>

					<xsl:choose>
						<xsl:when test="SOURCE = '*' ">
							<SOURCE>
								<xsl:value-of select ="''"/>
							</SOURCE>
						</xsl:when>
						<xsl:otherwise>
							<SOURCE>
								<xsl:value-of select="SOURCE"/>
							</SOURCE>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="CUSIP = '*' ">
							<CUSIP>
								<xsl:value-of select ="''"/>
							</CUSIP>
						</xsl:when>
						<xsl:otherwise>
							<CUSIP>
								<xsl:value-of select="CUSIP"/>
							</CUSIP>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="ISIN = '*' ">
							<ISIN>
								<xsl:value-of select ="''"/>
							</ISIN>
						</xsl:when>
						<xsl:otherwise>
							<ISIN>
								<xsl:value-of select="ISIN"/>
							</ISIN>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="SEDOL = '*' ">
							<SEDOL>
								<xsl:value-of select ="''"/>
							</SEDOL>
						</xsl:when>
						<xsl:otherwise>
							<SEDOL>
								<xsl:value-of select="SEDOL"/>
							</SEDOL>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="BloombergSymbol = '*' ">
							<BloombergTicker>
								<xsl:value-of select ="''"/>
							</BloombergTicker>
						</xsl:when>
						<xsl:otherwise>
							<BloombergTicker>
								<xsl:value-of select="BloombergSymbol"/>
							</BloombergTicker>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="SecurityName = '*' ">
							<SecurityName>
								<xsl:value-of select ="''"/>
							</SecurityName>
						</xsl:when>
						<xsl:otherwise>
							<SecurityName>
								<xsl:value-of select="SecurityName"/>
							</SecurityName>
						</xsl:otherwise>
					</xsl:choose>				

					<SecurityType>
						<xsl:choose>
							<xsl:when test="SecurityType = '*' ">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SecurityType"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityType>

					<PositionDate>
						<xsl:value-of select="PositionDate"/>
					</PositionDate>

					<PositionType>
						<xsl:value-of select="PositionType"/>
					</PositionType>

					<Price>					
						<xsl:value-of select='format-number(CostPerShare, "#.00")' />
					</Price>

					<Quantity>
						<xsl:value-of select="Quantity"/>
					</Quantity>

					<LastPrice>						
						<xsl:value-of select='format-number(LastPrice, "#.00")' />
					</LastPrice>

					<ExchangeRate>						
						<xsl:value-of select='format-number(ExchangeRate, "#.00")' />
					</ExchangeRate>

					<PorfolioBaseCurrency_MV>
						<xsl:value-of select='format-number(MarketValueBase, "#.00")' />
					</PorfolioBaseCurrency_MV>


					<Multiplier>
						<xsl:value-of select="Multiplier"/>
					</Multiplier>
					
					<OCCCode>
						<xsl:choose>
							<xsl:when test="OCCCode = '*' ">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="OCCCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</OCCCode>					

					<TotalDayPNL>
						<xsl:value-of select='format-number(TotalDayPNL, "#.00")' />
					</TotalDayPNL>									

					<!-- this is also for internal purpose-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
