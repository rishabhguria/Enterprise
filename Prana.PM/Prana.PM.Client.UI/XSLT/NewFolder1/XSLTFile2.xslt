<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<!--<xsl:if test="number(COL9) and COL1!='11221094'">-->
					<xsl:if test="number(COL15)"><!--</xsl:if>-->

					<PositionMaster>

						<!--Put Account/Fund here-->
						<Account>
							<xsl:value-of select="COL2"/>
						</Account>

						<!--Put Symbol/Ticker here-->
						<Symbol>
							<xsl:value-of select="COL6"/>
						</Symbol>

						<!--<SEDOL>
							
						</SEDOL>-->

						<!--Put Description here-->
						<PBSymbol>
							<xsl:value-of select="COL8"/>
						</PBSymbol>

						<!--Put Average Price here-->
						<CostBasis>
							<xsl:value-of select="COL18"/>
						</CostBasis>


						<!--Put Trade Date here-->
						<PositionStartDate>
							<!--<xsl:value-of select="COL8"/>-->
						</PositionStartDate>

						<!--Put Quantity here-->
						<NetPosition>
							<xsl:value-of select="COL15"/>
						</NetPosition>

						<!--Put Side here , Value should be 1 in case of 'Buy', 2 in case of 'Sell' , 5 in case of 'Sell short' , A in case of 'Buy to Open' , B in case of 'Buy to Close' , C in case of 'Sell to Open' and D in case of 'Sell to Close'-->
						<SideTagValue>
							<!--<xsl:choose>
								<xsl:when test="COL7='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL7='CoverShort'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="COL7='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL7='SellShort'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
							</xsl:choose>-->
						</SideTagValue>




					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>