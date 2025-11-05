<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/DocumentElement">
		
		<DocumentElement>
			
			<xsl:for-each select="//PositionMaster">		
				
				<xsl:if test ="normalize-space(COL9)!='Cash and Equivalents'and number(COL12)">
					
					<PositionMaster>
						
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Gs']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME!=''">
								<AccountName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(substring(COL8,2))"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
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
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL9 = 'Options' and COL4 != ''">
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="concat(COL2, 'U')"/>
										</IDCOOptionSymbol>
									</xsl:when>
									<xsl:when test ="COL9 = 'Options' and COL4 = ''">
										<Symbol>
											<xsl:value-of select="concat(substring(COL3,1,2), ' ',substring(COL3,3,2),substring(COL6,1,1), number(substring(COL5,4))*1000)"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="COL7"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<xsl:variable name="varQty">
							<xsl:value-of select="COL12"/>
						</xsl:variable>
						
						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varQty &lt; 0">
									<xsl:value-of select="$varQty*-1"/>
								</xsl:when>
								<xsl:when test="$varQty &gt; 0">
									<xsl:value-of select="$varQty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:choose>
							<xsl:when test="COL12 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL12 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL15"/>
						</xsl:variable>
						
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varAvgPrice &lt; 0">
									<xsl:value-of select="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test="$varAvgPrice &gt; 0">
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select ="''"/>
						</PositionStartDate>						

					</PositionMaster>
					
				</xsl:if>
				
			</xsl:for-each>
			
		</DocumentElement>
		
	</xsl:template>
	
</xsl:stylesheet>
