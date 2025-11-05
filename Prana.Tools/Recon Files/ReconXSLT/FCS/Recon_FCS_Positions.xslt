<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/NewDataSet">
		<NewDataSet>
			<xsl:for-each select="//Comparision">
				<xsl:if test ="number(COL5)" >
					<PositionMaster>

						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

						<Symbol>
							<xsl:value-of select="COL16"/>
						</Symbol>


						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number(COL6)">
									<xsl:value-of select="COL6"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="number(COL7)">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<xsl:variable name="varQuant">
							<xsl:value-of select="number(COL5)"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$varQuant &gt; 0">
									<xsl:value-of select="$varQuant"/>
								</xsl:when>
								<xsl:when test="$varQuant &lt; 0">
									<xsl:value-of select="$varQuant * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<Side>
							<xsl:choose>
								<xsl:when test="$varQuant &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varQuant &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</NewDataSet>
	</xsl:template>
</xsl:stylesheet>


