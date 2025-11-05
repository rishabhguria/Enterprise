<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="translate(COL1,'&quot;','')"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CORMARK']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
					<xsl:choose>
						<xsl:when test="COL2 != ''">
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
							<CUSIP>
								<xsl:value-of select="COL2"/>
							</CUSIP>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
							<CUSIP>
								<xsl:value-of select="''"/>
							</CUSIP>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:choose>
						<xsl:when  test="boolean(number(COL11))">
							<CostBasis>
								<xsl:value-of select="COL11"/>
							</CostBasis>
						</xsl:when >
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose >
					<xsl:choose>
						<xsl:when test="COL6 != 'STOCK_RECORD_DATE'">
							<PositionStartDate>
								<xsl:value-of select="concat(substring(COL6,3,2),'/',substring(COL6,5,2),'/',concat('20',substring(COL6,1,2)))"/>
							</PositionStartDate>
						</xsl:when>
						<xsl:otherwise>
							<PositionStartDate>
								<xsl:value-of select="''"/>
							</PositionStartDate>
							
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="COL7!='STOCK_RECORD_CURRENT'">
							<xsl:choose>
								<xsl:when test="COL7 &lt; 0">
									<NetPosition>
										<xsl:value-of select="COL7*(-1)"/>
									</NetPosition>
								</xsl:when>
								<xsl:when test="COL7 &gt; 0">
									<NetPosition>
										<xsl:value-of select="COL7"/>
									</NetPosition>
								</xsl:when>
								<xsl:otherwise>
									<NetPosition>
										<xsl:value-of select="0"/>
									</NetPosition>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="COL7!='STOCK_RECORD_CURRENT'">
							<xsl:choose>
								<xsl:when test="COL7 &lt; 0">
									<SideTagValue>
										<xsl:value-of select="'2'"/>
									</SideTagValue>
								</xsl:when>


								<xsl:when test="COL7 &gt; 0">
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
						</xsl:when>
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose>
			</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


