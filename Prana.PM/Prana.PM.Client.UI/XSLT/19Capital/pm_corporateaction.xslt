<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<DocumentElement>
		<xsl:for-each select="//PositionMaster">
			<xsl:if test="number(COL7)">
			<PositionMaster>
			
				<AccountName>
          <xsl:value-of select="''"/>
				</AccountName>

				<PositionStartDate>
          <xsl:value-of select="''"/>
				</PositionStartDate>

				<SideTagValue>
					<xsl:choose>
						<xsl:when test="COL6='Long' and substring(COL3, 1,2) = 'O:'">
							<xsl:value-of select="'A'"/>
						</xsl:when>
            <xsl:when test="COL6='Short' and substring(COL3, 1,2) = 'O:'">
              <xsl:value-of select="'C'"/>
            </xsl:when>
            <xsl:when test="COL6='Long' ">
              <xsl:value-of select="'1'"/>
            </xsl:when>
            <xsl:when test="COL6='Short' ">
              <xsl:value-of select="'5'"/>
            </xsl:when>
						
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</SideTagValue>

				<NetPosition>
					<xsl:choose>
						<xsl:when  test="number(normalize-space(COL7)) &gt; 0">
							<xsl:value-of select="COL7"/>
						</xsl:when>
						<xsl:when test="number(normalize-space(COL7)) &lt; 0">
							<xsl:value-of select="COL7* (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</NetPosition>

				<OriginalPurchaseDate>
					<xsl:value-of select ="COL2"/>
				</OriginalPurchaseDate>

				<Symbol>
					<xsl:choose>
						<xsl:when  test="normalize-space(COL3) != ''">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</Symbol>

				<CostBasis>
					<xsl:value-of select ="number(COL10) div COL7"/>
				</CostBasis>

				

				

			</PositionMaster>
			</xsl:if>
		</xsl:for-each>

	</DocumentElement>
</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>


</xsl:stylesheet> 
