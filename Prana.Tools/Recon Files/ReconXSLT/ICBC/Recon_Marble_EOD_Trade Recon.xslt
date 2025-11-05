<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">
				<xsl:if test="number(COL2)">
					<PositionMaster>
						<xsl:variable name="PB_Symbol">
							<xsl:value-of select = "normalize-space(substring-before(COL5,' Equity'))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME1">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="translate(normalize-space(substring-before(COL5,' Equity')), '/', '.')"/>
						</xsl:variable>

						<xsl:variable name="PB_ExchangeCODE">

							<xsl:value-of select="substring-after($varSymbol,' ')"/>
							<!--<xsl:choose>
								<xsl:when test="contains($varSymbol,' ')">
									<xsl:value-of select="substring-after($varSymbol,' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varSymbol"/>
								</xsl:otherwise>
							</xsl:choose>-->

						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
						</xsl:variable>




						<xsl:choose>
							<xsl:when test="normalize-space(COL5)='Unknown'">
								<ISIN>
									<xsl:value-of select="COL3"/>
								</ISIN>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="substring-after($varSymbol,' ') = 'US'">
										<ISIN>
											<xsl:value-of select="''"/>
										</ISIN>
										<Symbol>
											<xsl:value-of select="normalize-space(substring-before($varSymbol,'US'))"/>
										</Symbol>

									</xsl:when>
									<xsl:when test="$PRANA_SYMBOL_NAME='' and $PRANA_SYMBOL_NAME1=''">
										<ISIN>
											<xsl:value-of select="''"/>
										</ISIN>
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
									</xsl:when>

									<xsl:when test="$PRANA_SYMBOL_NAME!='' and $PRANA_SYMBOL_NAME1=''">
										<ISIN>
											<xsl:value-of select="''"/>
										</ISIN>
										<Symbol>
											<xsl:value-of select="concat(substring-before($varSymbol,' '),$PRANA_SYMBOL_NAME)"/>
										</Symbol>
									</xsl:when>

									<xsl:when test="$PRANA_SYMBOL_NAME1!=''">
										<ISIN>
											<xsl:value-of select="''"/>
										</ISIN>
										<Symbol>
											<xsl:value-of select="$PRANA_SYMBOL_NAME1"/>
										</Symbol>
									</xsl:when>

									<!--<xsl:when test="$varSymbol='/'">
											<ISIN>
												<xsl:value-of select="''"/>
											</ISIN>
											<Symbol>
												<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
											</Symbol>
											
										</xsl:otherwise>
									</xsl:when>-->

									<!--<xsl:otherwise>
										<ISIN>
											<xsl:value-of select="''"/>
										</ISIN>
										<Symbol>
											<xsl:value-of select="$varSymbol"/>
										</Symbol>

									</xsl:otherwise>-->
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>


						<AccountName>
							<xsl:value-of select="'Marble Bar'"/>
						</AccountName>

						<Quantity>
							<xsl:choose>
								<xsl:when  test="number(COL2) &gt; 0">
									<xsl:value-of select="COL2"/>
								</xsl:when>
								<xsl:when test="number(COL2) &lt; 0">
									<xsl:value-of select="COL2* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>


						<AvgPX>

							<xsl:choose>
								<xsl:when  test="number(COL7) &gt; 0">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:when  test="number(COL7) &lt; 0">
									<xsl:value-of select="COL7*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<!--<PositionStartDate>
							<xsl:value-of select="COL9"/>
						</PositionStartDate>-->


						<Side>
							<xsl:choose>
								<xsl:when test="normalize-space(COL1)='B'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Sell'"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<Description>
							<xsl:value-of select="COL6"/>
						</Description>


						<SMRequest>
							<xsl:value-of select="'TRUE'"/>
						</SMRequest>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
			</xsl:template>

</xsl:stylesheet> 
