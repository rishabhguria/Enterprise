<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="substring(COL1,22,8)='19210400' or substring(COL1,22,8)='19210327'">
				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="substring(COL1,22,8)"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME=''">
							<AccountName>
								<xsl:value-of select='$PB_FUND_NAME'/>
							</AccountName>
						</xsl:when>
						<xsl:otherwise>
							<AccountName>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</AccountName>
						</xsl:otherwise>
					</xsl:choose >
					<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(substring(COL1,65,12))"/>
					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SUNGARD']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
					</xsl:variable>
					<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME != ''">
						<Symbol>
							<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
						</Symbol>
						</xsl:when>
						<xsl:otherwise>
					<Symbol>
						<xsl:value-of select="normalize-space(substring(COL1,65,12))"/>
					</Symbol>
						</xsl:otherwise>
					</xsl:choose >


					<PBSymbol>
						<xsl:value-of select="normalize-space(substring(COL1,65,12))"/>
					</PBSymbol>


					<xsl:choose>
						<xsl:when  test="number(normalize-space(substring(COL1,82,14)))">
							<CostBasis>
								<xsl:value-of select="normalize-space(substring(COL1,82,14))"/>
							</CostBasis>
						</xsl:when >
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose >
					
					<PositionStartDate>
						<xsl:value-of select="concat(substring(COL1,31,4),'/',substring(COL1,35,2),'/',substring(COL1,37,2))"/>
					</PositionStartDate>

					<xsl:choose>
						<xsl:when  test="number(normalize-space(substring(COL1,118,16)))">
							<NetPosition>
								<xsl:value-of select="normalize-space(substring(COL1,118,16))"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>


					<Commission>
						<xsl:value-of select="normalize-space(substring(COL1,96,11))"/>
					</Commission>

					<xsl:choose>
						<xsl:when test="substring(COL1,134,1) = 'B'and substring(COL1,30,1) = '3'">
							<SideTagValue>

								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="substring(COL1,134,1) = 'B'and substring(COL1,30,1) != '3'">
							<SideTagValue>

								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when>
						
						<!--<xsl:when test="substring(COL1,134,1) = 'S'">
							<SideTagValue>
								--><!-- Sell to Open--><!--
								<xsl:value-of select="'2'"/>
							</SideTagValue>-->
						
							<xsl:when test="substring(COL1,134,1) = 'S' and substring(COL1,30,1) = '3'">
								<SideTagValue>
									<!-- Sell to Open-->
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
						<xsl:when test="substring(COL1,134,1) = 'S' and substring(COL1,30,1) != '3'">
							<SideTagValue>
								<!-- Sell to Open-->
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
						<xsl:value-of select="normalize-space(substring(COL1,145,4))"/>
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


