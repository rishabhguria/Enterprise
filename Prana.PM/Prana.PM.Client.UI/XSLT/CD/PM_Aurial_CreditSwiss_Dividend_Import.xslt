<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="substring(COL123,1,3)='DIV'">
					<PositionMaster>

						<!--<xsl:variable name="PB_Name">
							<xsl:value-of select="'Credit_Swiss'"/>
						</xsl:variable>-->



						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'CS'"/>
						</xsl:variable>

						<xsl:variable name="PB_COMPANY_NAME" select="COL27"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=normalize-space($PB_COMPANY_NAME)]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>


						<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(substring-before(COL27,'Equity'))"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_CODE">
							<xsl:value-of select="substring-after($varSymbol,' ')"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
						</xsl:variable>
						
						
								<Symbol>
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="$PRANA_SUFFIX_NAME!=''">
													<xsl:value-of select="concat(substring-before($varSymbol,' '),$PRANA_SUFFIX_NAME)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$varSymbol"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
									<xsl:value-of select="COL27"/>
								</Symbol>
						

						<xsl:variable name ="varDividend">
							<xsl:value-of select ="COL19"/>
						</xsl:variable>

				

						<!--<NetPosition>
							<xsl:choose>
								<xsl:when test="boolean(number($varDividend))">
									<xsl:value-of select="concat($varDividendInt,'.',$varDividendFrac)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>-->


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

						<xsl:variable name="varDate">
							<xsl:value-of select="substring(COL1,72,8)"/>
						</xsl:variable>

						<PayoutDate>
							<xsl:value-of select="COL16"/>
						</PayoutDate>

						<ExDate>
							<xsl:value-of select="COL15"/>
						</ExDate>

						<!--<DeclarationDate>
							<xsl:value-of select="concat(substring($varDate,1,2),'/',substring($varDate,3,2),'/',substring($varDate,5,4))"/>
						</DeclarationDate>-->

						<RecordDate>
							<xsl:value-of select="COL17"/>
						</RecordDate>

						<Description>
							<xsl:value-of select="'Dividend'"/>
						</Description>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
