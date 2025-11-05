<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:variable name="varSecurityType" select="COL7"/>
				<xsl:if test ="$varSecurityType='B' or  $varSecurityType='0'">
					<PositionMaster>

						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

						<xsl:choose>
							<xsl:when test ="$varSecurityType='B'">
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(substring(COL29,1,21),'U')"/>
								</IDCOOptionSymbol>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="normalize-space(COL29)"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>


						<PBSymbol>
							<xsl:value-of select ="COL29"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when  test="number(COL14)">
								<CostBasis>
									<xsl:value-of select="COL14"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >


						<xsl:variable name ="varDate">
							<xsl:value-of select ="COL3"/>
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
							<xsl:when  test="number(COL15)">
								<NetPosition>
									<xsl:value-of select="COL15"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL18))">
								<Commission>
									<xsl:value-of select="COL18"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL21))">
								<SecFees>
									<xsl:value-of select="COL21"/>
								</SecFees>
							</xsl:when>
							<xsl:otherwise>
								<SecFees>
									<xsl:value-of select="0"/>
								</SecFees>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL22))">
								<Fees>
									<xsl:value-of select="COL22"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select="0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varSide">
							<xsl:value-of select="normalize-space(COL13)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varSide = 'B' and $varSecurityType='0'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='S'and $varSecurityType='0'">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SS' and $varSecurityType='0'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='CS' ">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide = 'B' and $varSecurityType='B'">
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='S'and $varSecurityType='B'">
								<SideTagValue>
									<xsl:value-of select="'D'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SS'and $varSecurityType='B'">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:variable name = "PB_Broker" >
							<xsl:value-of select="normalize-space(COL23)"/>
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
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


