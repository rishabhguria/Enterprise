<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL1!='Fund'">
					<PositionMaster>
						<!--   Fund -->
						<!--<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Jasincap']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
						</xsl:choose>-->

						<!--<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(COL8)"/>
						</xsl:variable>
						<xsl:variable name ="varUnderlying">
							<xsl:value-of select ="normalize-space(COL10)"/>
						</xsl:variable>
						<--><!--PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>-->
						<AccountName>
							<xsl:value-of select="COL1"/>
						</AccountName>
								<Side>
									<xsl:value-of select="COL5"/>
								</Side>


								<Quantity>
									<xsl:value-of select="COL7"/>
								</Quantity>


								<AvgPX>
									<xsl:value-of select="COL8"/>
								</AvgPX>

						<Symbol>
							<xsl:value-of select="COL3"/>
						</Symbol>


						<!--<xsl:choose>
							<xsl:when test ="COL14 &lt; 0">
								<NetNotionalValue>
									<xsl:value-of select="(COL20)"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0">
								<NetNotionalValue>
									<xsl:value-of select="((COL20)*(-1))"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>-->



						<!--COMMISSION-->
						<!--<xsl:choose>
							<xsl:when test ="boolean(number(COL15))">
								<Commission>
									<xsl:value-of select="COL15"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<Fees>
							<xsl:value-of select="COL16 + COL17"/>
						</Fees>

						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL3)"/>

						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<PBSymbol>
							<xsl:value-of select="normalize-space(COL8)"/>
						</PBSymbol>-->

				
						<!--<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Jefferies']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							--><!--<xsl:when test ="COL8 = 'Options'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(COL1,'U')"/>
								</IDCOOptionSymbol>
							</xsl:when>--><!--
							
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL8"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
							
						</xsl:choose>-->




						<!--<Symbol>
							<xsl:value-of select="normalize-space(COL8)"/>
						</Symbol >-->

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
