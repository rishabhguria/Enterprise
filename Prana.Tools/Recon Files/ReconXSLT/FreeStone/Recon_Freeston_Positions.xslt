<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="COL2 !='AssetClass'">
				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL1"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
					</xsl:choose >

					<xsl:variable name="varPBSymbol" select="COL4"/>
					<xsl:choose>
						<xsl:when test ="COL2='OPT'">
							<IDCOOptionSymbol>
								<xsl:value-of select="concat($varPBSymbol,'U')"/>
							</IDCOOptionSymbol>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
						</xsl:when>

						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="$varPBSymbol"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>
						</xsl:otherwise>
					</xsl:choose>


					<PBSymbol>
						<xsl:value-of select ="COL4"/>
					</PBSymbol>


					<PBAssetName>
						<xsl:value-of select="COL2"/>
					</PBAssetName>


					<xsl:choose>
						<xsl:when test="boolean(number(COL8))">
							<AvgPX>
								<xsl:value-of select="number(COL8)"/>
							</AvgPX>
						</xsl:when>
						<xsl:otherwise>
							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>
						</xsl:otherwise>
					</xsl:choose>

					<PositionStartDate>
						<xsl:value-of select="''"/>
					</PositionStartDate>



					<xsl:choose>
						<xsl:when  test="boolean(number(COL7))">
							<Quantity>
								<xsl:value-of select="COL7"/>
							</Quantity>
						</xsl:when>
						<xsl:otherwise>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test="COL3= 'Long' and COL2 !='OPT'">
							<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>
						</xsl:when>

						<xsl:when test="COL3= 'Short' and COL2 !='OPT'">
							<Side>
								<xsl:value-of select="'Short Sell'"/>
							</Side>
						</xsl:when>
						<xsl:when test="COL3= 'Long' and COL2='OPT'">
							<Side>
								<xsl:value-of select="'Buy to Open'"/>
							</Side>
						</xsl:when>
						<xsl:when test="COL3= 'Short' and COL2='OPT'">
							<Side>
								<xsl:value-of select="'Sell to open'"/>
							</Side>
						</xsl:when>
						<xsl:otherwise>
							<Side>
								<xsl:value-of select="''"/>
							</Side>
						</xsl:otherwise>
					</xsl:choose>

					<SMRequest>
						<xsl:value-of select ="'TRUE'"/>
					</SMRequest>

				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


