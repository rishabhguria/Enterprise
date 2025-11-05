<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">
				<xsl:if test="COL49=1 or COL49=12">
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:if test="COL1='038C35642'">
							<FundName>
								<xsl:value-of select="'UBS Trading'"/>
							</FundName>
						</xsl:if>
						<xsl:if test="COL1='038C35741'">
							<FundName>
								<xsl:value-of select="'Fund 2'"/>
							</FundName>
						</xsl:if>

						<!--  Symbol Region -->

						<!--<Symbol>
							<xsl:value-of select="Specify Column Name"/>
						</Symbol>-->

						<!-- Symbol Region Ends -->
						
						<!--   CUSIP -->
						<!-- Column 2 mapped with CUSIP-->
						<CUSIP>
							<xsl:value-of select="COL2"/>
						</CUSIP>

						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="COL3"/>
						</PBSymbol>
						
						<!-- Column 4 mapped with Side-->
						<xsl:if test="COL4='L'">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
							<!--<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>-->
						</xsl:if>
						<xsl:if test="COL4='S'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
							<!--<Side>
								<xsl:value-of select="'Sell short'"/>
							</Side>-->
						</xsl:if>

						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:if test="COL5 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL5*(-1)"/>
							</NetPosition>
						</xsl:if>
						<xsl:if test="COL5 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL5"/>
							</NetPosition>
						</xsl:if>

						<!-- we have Notional value (column 11) and quantity(column 5), so Average Price = Column11/column5-->
						<!-- format-number is a inbulit function so gives the value up to 4 decimal palces-->
						<xsl:if test="COL5 != 0">
							<CostBasis>
								<xsl:value-of select='format-number((COL11 div COL5), "###.0000")'/>
							</CostBasis>
						</xsl:if>

						<!--  Average Price Region -->

						<!--<CostBasis>
							<xsl:value-of select="Specify Column Name"/>
						</CostBasis>-->

						<!-- Symbol Region  -->

						<SEDOL>
							<xsl:value-of select="COL14"/>
						</SEDOL>

						<ISIN>
							<xsl:value-of select="COL23"/>
						</ISIN>

						<!-- Position Date mapped with the column 51 -->
						<PositionStartDate>
							<xsl:value-of select="COL51"/>
						</PositionStartDate>

						<RIC>
							<xsl:value-of select="COL52"/>
						</RIC>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>	
</xsl:stylesheet>
