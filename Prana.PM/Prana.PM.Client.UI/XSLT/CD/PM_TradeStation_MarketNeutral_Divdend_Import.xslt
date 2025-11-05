<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL4)">
					<PositionMaster>

						<!--<xsl:variable name="PB_Name">
							<xsl:value-of select="'Credit_Swiss'"/>
						</xsl:variable>-->



						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'CS'"/>
						</xsl:variable>

						<xsl:variable name="PB_COMPANY_NAME" select="COL2"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=normalize-space($PB_COMPANY_NAME)]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>-->

						<AccountName>
							<xsl:value-of select="''"/>
						</AccountName>

						<CurrencyID>
							<xsl:value-of select="2"/>
						</CurrencyID>


						<!--<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(substring-before(COL27,'Equity'))"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_CODE">
							<xsl:value-of select="substring-after($varSymbol,' ')"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
						</xsl:variable>-->


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL1"/>
								</xsl:otherwise>
							</xsl:choose>						
						</Symbol>


						<xsl:variable name ="varDividend">
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<Dividend>
							<xsl:choose>
								<xsl:when  test="number($varDividend) &gt; 0">
									<xsl:value-of select="$varDividend"/>
								</xsl:when>
								<xsl:when test="number($varDividend) &lt; 0">
									<xsl:value-of select="$varDividend* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Dividend>

					
						<PayoutDate>
							<xsl:value-of select="COL6"/>
						</PayoutDate>

						<ExDate>
							<xsl:value-of select="COL5"/>
						</ExDate>
					
						<Description>
							<xsl:value-of select="'Dividend'"/>
						</Description>

						<!--<PBSymbol>

							<xsl:value-of select="COL2"/>
						</PBSymbol>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
