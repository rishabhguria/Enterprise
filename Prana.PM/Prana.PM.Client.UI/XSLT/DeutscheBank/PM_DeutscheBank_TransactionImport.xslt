<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<!--<xsl:if test="substring(COL1,22,8)='19210558'">-->
					<!--or substring(COL,22,8)='19210558'-->
				<xsl:if test ="substring(COL1,6,1) !='A' and substring(COL1,6,1) !='Z'">
					
			
					<PositionMaster>

						<xsl:variable name="varPBSymbol" select="substring-before(COL14,'.')"/>
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
							<xsl:when  test="boolean(number(COL18))">
								<CostBasis>
									<xsl:value-of select="COL18"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >


						<xsl:variable name ="varDateString" select="normalize-space(COL16)"/>
						<xsl:variable name ="varDate">
							<xsl:value-of select ="concat(substring($varDateString,1,4),'/',substring($varDateString,5,2),'/',substring($varDateString,7,2))"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varDate !=''">
								<PositionStartDate>
									<xsl:value-of select="$varDate"/>
								</PositionStartDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<xsl:value-of select="'rr'"/>
								</PositionStartDate>
							</xsl:otherwise>
						</xsl:choose>						

						<xsl:choose>
							<xsl:when  test="boolean(number(COL17))and number(COL17) &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL17 * -1"/>
								</NetPosition>
							</xsl:when>
							<xsl:when  test="boolean(number(COL17))and number(COL17) &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL17"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when  test="number(COL17) &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when  test="number(COL17) &gt; 0">
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
					
				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


