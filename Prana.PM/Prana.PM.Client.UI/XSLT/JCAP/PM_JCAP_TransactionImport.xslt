<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				
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
							<xsl:when  test="boolean(number(COL8))">
								<CostBasis>
									<xsl:value-of select="COL8"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >


						
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
								<NetPosition>
									<xsl:value-of select="COL7 * -1"/>
								</NetPosition>
							</xsl:when>
							<xsl:when  test="boolean(number(COL7))and number(COL7) &gt; 0">
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
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when  test="COL6='Sell'">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name = "PB_Broker" >
							<xsl:value-of select="COL14"/>
						</xsl:variable>
						<xsl:variable name="PRANA_Broker">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='SUNGARD']/BrokerData[@PBBroker=$PB_Broker]/@PranaBrokerCode"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_Broker!=''">
								<CounterPartyID>
									<xsl:value-of select="$PRANA_Broker"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select="1"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>
					
					</PositionMaster>
				</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


