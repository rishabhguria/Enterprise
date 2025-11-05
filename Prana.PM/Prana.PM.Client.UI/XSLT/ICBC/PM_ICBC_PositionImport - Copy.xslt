<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>
			
			<xsl:for-each select ="//PositionMaster">

				<xsl:if test ="number(COL12)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'ML'"/>
						</xsl:variable>



						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="concat(COL2,COL3,COL4,COL5)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL21"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="COL11='OS'">
													<xsl:value-of select="''"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select ="COL9"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:choose>
										<xsl:when test="COL11='OS'">
											<xsl:value-of select="concat(COL9,'U')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</IDCOOptionSymbol>


						<xsl:variable name ="varNetPosition">
							<xsl:value-of select ="number(COL12)"/>
						</xsl:variable>

						<NetPosition>

							<xsl:choose>
								<xsl:when test ="$varNetPosition &lt;0">
									<xsl:value-of select ="$varNetPosition*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varNetPosition"/>
								</xsl:otherwise>
							</xsl:choose>

						</NetPosition>


						<SideTagValue>

							<xsl:choose>
								
								<xsl:when test="COL11='OS' and $varNetPosition &gt; 0">
									<xsl:value-of select ="'A'"/>
								</xsl:when>
								
								<xsl:when test="COL11='OS' and $varNetPosition &lt; 0">
									<xsl:value-of select ="'C'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:choose>
										
										<xsl:when test ="$varNetPosition &gt; 0">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
										
										<xsl:when test ="$varNetPosition &lt; 0">
											<xsl:value-of select ="'5'"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
										
									</xsl:choose>
								</xsl:otherwise>
								
							</xsl:choose> 
							
						</SideTagValue>


						<xsl:variable name ="varMarkPrice">
							<xsl:choose>
								<xsl:when test ="COL16 &lt;0">
									<xsl:value-of select ="COL16*-1"/>
								</xsl:when>

								<xsl:when test ="COL16 &gt;0">
									<xsl:value-of select ="COL16"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CostBasis>

							<xsl:choose>
								<xsl:when test="COL11='B' or COL11='F'">
									<xsl:value-of select="$varMarkPrice*100"/>
								</xsl:when>
								
								<xsl:when test="COL11='OS'">
									<xsl:value-of select="$varMarkPrice div 100"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:otherwise>
								
							</xsl:choose>
							

						</CostBasis>

						<PBSymbol>
							<xsl:choose>
								<xsl:when test ="COL21 !='*'">
									<xsl:value-of select ="COL21"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL9"/>
								</xsl:otherwise>
							</xsl:choose>
						</PBSymbol>

						<PositionStartDate>
							<xsl:value-of select="COL1"/>
						</PositionStartDate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
