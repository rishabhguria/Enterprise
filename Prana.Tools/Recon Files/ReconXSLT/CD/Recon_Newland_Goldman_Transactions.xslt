<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">

	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL13 != 'TradeQuantity' and COL13 != 0">
					<PositionMaster>

						<!--FUNDNAME SECTION-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<!-- SYMBOL, PBSYMBOL, COMPANYNAME SECTION -->
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>

						<PBSymbol>
							<xsl:value-of select="COL8"/>
						</PBSymbol>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select='$PRANA_SYMBOL_NAME'/>
								</Symbol>
							</xsl:when>
							<xsl:when test="starts-with(COL8,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL8)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL8,2,($varLength - 3)),' ',substring(COL8,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select='COL8'/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="COL13 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL13*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL13 &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL13"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="boolean(number(COL14))">
								<AvgPX>
									<xsl:value-of select="COL14"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL16))">
								<Commission>
									<xsl:value-of select="COL16"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!--GROSS NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL113))">
								<xsl:choose>
									<xsl:when test="COL113 &lt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL113*(-1)"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:when test="COL113 &gt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL113"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:otherwise>
										<GrossNotionalValue>
											<xsl:value-of select="0"/>
										</GrossNotionalValue>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<GrossNotionalValue>
									<xsl:value-of select="0"/>
								</GrossNotionalValue>
							</xsl:otherwise>
						</xsl:choose>

						<!--NET NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL17))">
								<xsl:choose>
									<xsl:when test="COL17 &lt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL17*(-1)"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:when test="COL17 &gt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL17"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:otherwise>
										<NetNotionalValue>
											<xsl:value-of select="0"/>
										</NetNotionalValue>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>




					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
