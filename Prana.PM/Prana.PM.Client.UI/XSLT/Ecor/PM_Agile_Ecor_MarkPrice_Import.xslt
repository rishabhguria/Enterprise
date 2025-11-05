<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<DocumentElement>

		<xsl:for-each select ="//PositionMaster">
			<xsl:if test ="number(COL26)">
				<PositionMaster>

					<xsl:variable name ="varMarkPrice">
						<xsl:value-of select ="COL26"/>
					</xsl:variable>

					<MarkPrice>
						<xsl:choose>
							<xsl:when test ="$varMarkPrice &lt;0">
								<xsl:value-of select ="$varMarkPrice*-1"/>
							</xsl:when>

							<xsl:when test ="$varMarkPrice &gt;0">
								<xsl:value-of select ="$varMarkPrice"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</MarkPrice>

					<Date>
						<xsl:value-of select ="substring(COL1,1,9)"/>
					</Date>

					<PBSymbol>
						<xsl:value-of select ="COL12"/>
					</PBSymbol>

						<xsl:variable name = "PB_SYMBOL_NAME" >
						<xsl:value-of select ="COL12"/>
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
								<xsl:when test="number(COL11)">

									<Symbol>
										<xsl:value-of select ="COL3"/>
									</Symbol>

									<IDCOOptionSymbol>
										<xsl:value-of select ="''"/>
									</IDCOOptionSymbol>
								</xsl:when>


								<xsl:when test ="COL5!='*'">
									<xsl:choose>
										<xsl:when test ="string-length(COL5) &gt; 15">
											
											<Symbol>
												<xsl:value-of select ="''"/>
											</Symbol>

											<IDCOOptionSymbol>
												<xsl:value-of select ="concat(COL5,'U')"/>
											</IDCOOptionSymbol>

										</xsl:when>
										
										
										<xsl:otherwise>

											<Symbol>
												<xsl:value-of select ="COL5"/>
											</Symbol>

											<IDCOOptionSymbol>
												<xsl:value-of select ="''"/>
											</IDCOOptionSymbol>

										</xsl:otherwise> 
										
									</xsl:choose> 
								</xsl:when>

								<xsl:otherwise>

									<Symbol>
										<xsl:value-of select ="COL12"/>
									</Symbol>

									<IDCOOptionSymbol>
										<xsl:value-of select ="''"/>
									</IDCOOptionSymbol>
									
								</xsl:otherwise>
							</xsl:choose> 
							
						</xsl:otherwise> 
						
					</xsl:choose>


				</PositionMaster>
			</xsl:if>
		</xsl:for-each>
	</DocumentElement>
</xsl:template>

</xsl:stylesheet> 
