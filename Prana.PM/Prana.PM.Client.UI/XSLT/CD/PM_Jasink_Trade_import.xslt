<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/DocumentElement">
		
		<DocumentElement>
			
			<xsl:for-each select="//PositionMaster">
				
				<xsl:if test="number(COL16)">
					
					<PositionMaster>
						<xsl:variable name="varPBName">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						




						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<!--<xsl:variable name="varPB_Name">
							<xsl:value-of select="'Jefferies'"/>
						</xsl:variable>-->

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<!--<AccountName>
							<xsl:value-of select='"Jasinkiewicz"'/>
						</AccountName>-->						

						<!--<xsl:choose>
							<xsl:when test ="COL14 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL14*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL14"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>-->

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="contains(COL25,'OPT')">
									<xsl:choose>
										<xsl:when test="COL6='Buy'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell Short'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="COL6='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL6='Buy'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="COL6='Sell Short'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="COL6='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>
							

						<NetPosition>
							<xsl:choose>
								<xsl:when test="number(COL16) &gt;0">
									<xsl:value-of select ="COL16"/>
								</xsl:when>
								<xsl:when test="number(COL16) &lt;0">
									<xsl:value-of select ="COL16*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<!--<xsl:variable name ="varGrossAmt">
							<xsl:choose>
								<xsl:when test ="number(COL20) &lt; 0">
									<xsl:value-of select="number(COL20)*(-1) - (number(COL15) + number(COL16) + number(COL17)) "/>
								</xsl:when>
								<xsl:when test ="number(COL20) &gt; 0">
									<xsl:value-of select="COL20 + (number(COL15) + number(COL16) + number(COL17))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="COL14 &lt; 0 ">
								<CostBasis>
									<xsl:value-of select="$varGrossAmt div COL14 *(-1)"/>
								</CostBasis>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0 ">
								<CostBasis>
									<xsl:value-of select="$varGrossAmt div COL14 "/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>-->

						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL14) &gt;0">
									<xsl:value-of select ="COL14"/>
								</xsl:when>
								<xsl:when test="number(COL14) &lt;0">
									<xsl:value-of select ="COL14*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
						
						<PBSymbol>
							<xsl:value-of select="normalize-space(COL5)"/>
						</PBSymbol>

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL != ''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test ="string-length(COL8) = 21">

									<xsl:value-of select ="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL10)"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol >

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL != ''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test ="string-length(COL8) = 21">
									<xsl:value-of select ="concat(COL8,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<PositionStartDate>
							<xsl:value-of select ="COL2"/>
						</PositionStartDate>

						<xsl:variable name="varCommission">
							<xsl:value-of select="COL17"/>
						</xsl:variable>
						
						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>


						<xsl:variable name="varFees">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="$varFees &gt; 0">
									<xsl:value-of select="$varFees"/>
								</xsl:when>
								<xsl:when test="$varFees &lt; 0">
									<xsl:value-of select="$varFees*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>


						<xsl:variable name="varStampDuty">
							<xsl:value-of select="COL18"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test="$varStampDuty &gt; 0">
									<xsl:value-of select="$varStampDuty"/>
								</xsl:when>
								<xsl:when test="$varStampDuty &lt; 0">
									<xsl:value-of select="$varStampDuty*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>


					</PositionMaster>
					
				</xsl:if >
				
			</xsl:for-each>
			
		</DocumentElement>
		
	</xsl:template>


</xsl:stylesheet>
