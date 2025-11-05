<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<!-- Put = 0,Call = 1 , Here First call/put code then 2 characters for month code -->
		<!-- Call month Codes e.g. 101 represents 1=Call, 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth=01">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=02">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=03">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=04">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=05">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=06">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=07">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=08">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=09">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=10">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL1"/>
					</xsl:variable>
					<FundName>
						<xsl:value-of select="COL1"/>
					</FundName>


					<xsl:variable name = "VarFutureExpirationMonth" >
						<xsl:value-of select="normalize-space(COL11)"/>
					</xsl:variable>

					<xsl:variable name = "VarFutureRootSymbol" >
						<xsl:value-of select="normalize-space(COL21)"/>
					</xsl:variable>

					<xsl:variable name = "VarFutureExpirationYear" >
						<xsl:value-of select="normalize-space(substring(COL12,4,1))"/>
					</xsl:variable>

					<xsl:variable name = "varFutureMonthCode" >
						<xsl:call-template name="MonthCode">
							<xsl:with-param name="varMonth" select="$VarFutureExpirationMonth" />
						</xsl:call-template>
					</xsl:variable>


					<xsl:variable name="PRANA_Root_Symbol">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='RJO']/SymbolData[@PBCompanyName=$VarFutureRootSymbol]/@PranaSymbol"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="COL2 != ''">
							<Symbol>
								<xsl:value-of select="COL2"/>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="COL2"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when  test="boolean(number(COL5))">
							<CostBasis>
								<xsl:value-of select="COL5"/>
							</CostBasis>
						</xsl:when >
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose >



					<PositionStartDate>
						<xsl:value-of select="COL6"/>
					</PositionStartDate>





					<xsl:choose>
						<xsl:when test="COL3 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL3*(-1)"/>
							</NetPosition>
						</xsl:when>
						<xsl:when test="COL3 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL3"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>

					<!--<xsl:choose>
						<xsl:when test="COL9 &lt; 0">
							<Commission>
								<xsl:value-of select="COL9*(-1)"/>
							</Commission>
						</xsl:when>
						<xsl:when test="COL9 &gt; 0">
							<Commission>
								<xsl:value-of select="COL9"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>-->


					<SideTagValue>
						<xsl:value-of select="COL4"/>
					</SideTagValue>


				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


