<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="COL1 != 'Trade Date'">
					<PositionMaster>
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL10"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='LUCAS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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


						<Symbol>
							<xsl:value-of select="normalize-space(COL3)"/>
						</Symbol>



						<PBSymbol>
							<xsl:value-of select ="COL3"/>
						</PBSymbol>


						<xsl:choose>
							<xsl:when  test="number(COL6)">
								<CostBasis>
									<xsl:value-of select="COL6"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:variable name ="varDate">
							<xsl:value-of select ="concat(substring(COL1,1,4),'/',substring(COL1,5,2),'/',substring(COL1,7,2))"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varDate !='' and $varDate != '*' and $varDate != '//'">
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
							<xsl:when  test="boolean(number(COL4)) and number(COL4) &lt; 0 ">
								<NetPosition>
									<xsl:value-of select="number(COL4)* -1"/>
								</NetPosition>
							</xsl:when>
							<xsl:when  test="boolean(number(COL4)) and number(COL4) &gt; 0">
								<NetPosition>
									<xsl:value-of select="number(COL4)"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL8))">
								<Commission>
									<xsl:value-of select="COL8"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varSide">
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varSide = 'Buy' ">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='Sell'">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='Short Sell' ">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='Buy to Cover'">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
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


