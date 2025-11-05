<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//Comparision">
				<xsl:if test="COL10 = 'T' and COL11 = 'EQ' and contains(normalize-space(COL8),'Sensato') != false">
					<PositionMaster>

						<!--FUND NAME-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SENSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<!--Trade Date-->
						<!--

							<xsl:choose>
								<xsl:when test="string-length(COL2)= 8">
									<Date>
										<xsl:value-of select ="concat(substring(COL2,5,2),'/',substring(COL2,7,2),'/',substring(COL2,1,4))"/>
									</Date>
								</xsl:when>
								<xsl:otherwise>
									<Date>
										<xsl:value-of select ="''"/>
									</Date>
								</xsl:otherwise>
							</xsl:choose>-->


						<!--SYMBOL-->


						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL22)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL22"/>
								</PBSymbol>
                <SEDOL>
                  <xsl:value-of select="''"/>
                </SEDOL>
              </xsl:when>
							<xsl:when test="COL18 != ''">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL22"/>
								</PBSymbol>
                <SEDOL>
                  <xsl:value-of select="COL18"/>
                </SEDOL>
              </xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL22"/>
								</PBSymbol>
                <SEDOL>
                  <xsl:value-of select="''"/>
                </SEDOL>
              </xsl:otherwise>
						</xsl:choose>





						<!--ORDER SIDE-->

						<xsl:choose>
							<xsl:when test="COL25= 'S'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL25= 'L'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="COL25"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>



						<!--QUANTITY-->


						<xsl:choose>
							<xsl:when test="COL26 &lt; 0 and number(COL26)">
								<Quantity>
									<xsl:value-of select="COL26*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="COL26 &gt; 0 and number(COL26)">
								<Quantity>
									<xsl:value-of select="COL26"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!--Trade Price-->


						<xsl:choose>
							<xsl:when  test="number(COL29)">
								<AvgPX>
									<xsl:value-of select="COL29"/>
								</AvgPX>
							</xsl:when >
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose >

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>




					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->

</xsl:stylesheet>
