<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<PositionMaster>
					<FundName>
						<xsl:value-of select="''"/>
					</FundName>

					<xsl:variable name="varPBSymbol" select="COL3"/>
					<xsl:choose>
						<xsl:when test ="$varPBSymbol!=''">
							<Symbol>
								<xsl:value-of select ="$varPBSymbol"/>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="$varPBSymbol!=''">
							<PBSymbol>
								<xsl:value-of select ="$varPBSymbol"/>
							</PBSymbol>
						</xsl:when>
						<xsl:otherwise>
							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>
						</xsl:otherwise>
					</xsl:choose>

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

					<xsl:variable name ="varDate" select="normalize-space(COL2)"/>

					<xsl:choose>
						<xsl:when test ="$varDate !=''">
							<PositionStartDate>
								<xsl:value-of select="$varDate"/>
							</PositionStartDate>
						</xsl:when>
						<xsl:otherwise>
							<PositionStartDate>
								<xsl:value-of select="''"/>
							</PositionStartDate>
						</xsl:otherwise>
					</xsl:choose>



					<xsl:choose>
						<xsl:when  test="boolean(number(COL7))and number(COL7) &lt; 0">
							<Quantity>
								<xsl:value-of select="COL7 * -1"/>
							</Quantity>
						</xsl:when>
						<xsl:when  test="boolean(number(COL7))and number(COL7) &gt; 0">
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
						<xsl:when test="boolean(number(COL9))">
							<Commission>
								<xsl:value-of select="COL9"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="boolean(number(COL10))">
							<Fees>
								<xsl:value-of select="number(COL10)"/>
							</Fees>
						</xsl:when>
						<xsl:otherwise>
							<Fees>
								<xsl:value-of select="0"/>
							</Fees>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when  test="COL6='Buy'">
							<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>
						</xsl:when>
						<xsl:when  test="COL6='Sell'">
							<Side>
								<xsl:value-of select="'Sell'"/>
							</Side>
						</xsl:when>
						<xsl:otherwise>
							<Side>
								<xsl:value-of select="''"/>
							</Side>
						</xsl:otherwise>
					</xsl:choose>

				</PositionMaster>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


