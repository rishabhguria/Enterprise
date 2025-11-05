<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<!--<xsl:if test="substring(COL1,22,8)='19210558'">-->
				<!--or substring(COL,22,8)='19210558'-->
				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="substring(COL1,21,12)"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

					<xsl:variable name="varPBSymbol" select="normalize-space(substring(COL1,131,12))"/>
					<xsl:variable name ="varCallPutCode">
						<xsl:choose>
							<xsl:when test ="normalize-space(substring(COL1,668,1))='C'">
								<xsl:value-of select ="'1'"/>
							</xsl:when>
							<xsl:when test ="normalize-space(substring(COL1,668,1))='P'">
								<xsl:value-of select ="'0'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="$varCallPutCode !=''">
							<IDCOOptionSymbol>
								<xsl:value-of select="concat(substring(COL1,1553,21),'U')"/>
							</IDCOOptionSymbol>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="$varPBSymbol"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="$varCallPutCode!=''">
							<PBSymbol>
								<xsl:value-of select ="substring(COL1,1553,21)"/>
							</PBSymbol>
						</xsl:when>
						<xsl:otherwise>
							<PBSymbol>
								<xsl:value-of select="normalize-space(substring(COL1,131,12))"/>
							</PBSymbol>
						</xsl:otherwise>
					</xsl:choose>
					
						<xsl:choose>
							<xsl:when  test="number(normalize-space(substring(COL1,843,18)))">
								<CostBasis>
									<xsl:value-of select="normalize-space(substring(COL1,843,18))"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >

						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<xsl:choose>
							<xsl:when  test="boolean(number(normalize-space(substring(COL1,797,19))))">
								<NetPosition>
									<xsl:value-of select="normalize-space(substring(COL1,797,19))"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>


					<xsl:choose>
						<xsl:when test="substring(COL1,796,1)='+'and $varCallPutCode=''">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when>
						
						<xsl:when test="substring(COL1,796,1)='-' and $varCallPutCode=''">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="substring(COL1,796,1)='+' and $varCallPutCode != ''">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="substring(COL1,796,1)='-' and $varCallPutCode != ''">
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
						<xsl:value-of select="normalize-space(substring(COL1,65,6))"/>
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
				<!--</xsl:if>-->
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


