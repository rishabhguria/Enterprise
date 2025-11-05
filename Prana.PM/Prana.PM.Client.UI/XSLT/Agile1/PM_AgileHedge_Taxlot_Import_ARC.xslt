<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(translate(COL6,',','')) and normalize-space(COL4)!='Cash'">
					<PositionMaster>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL8"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BTIG']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL_NAME !=''">
								<Symbol>
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select ="''"/>
								</IDCOOptionSymbol>
							</xsl:when>

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="contains(COL5,'Option')">
										<Symbol>
											<xsl:value-of select ="''"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select ="concat(COL7,'U')"/>
										</IDCOOptionSymbol>
									</xsl:when>
									
									<xsl:otherwise>								
										<Symbol>
											<xsl:value-of select ="normalize-space(COL7)"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select ="''"/>
										</IDCOOptionSymbol>							
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					
						<AccountName>
							<xsl:value-of select="''"/>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select ="COL1"/>
						</PositionStartDate>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varQuantity">
							<xsl:value-of select="number(translate(COL6,',',''))"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*-1'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varSide">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<SideTagValue>
							<xsl:choose>

								<xsl:when test="contains(COL5,'Options')!=false">
									<xsl:choose>
										<xsl:when test="$varSide='Long'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$varSide='Short'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSide='Long'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$varSide='Short'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>

							
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="COL8"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
