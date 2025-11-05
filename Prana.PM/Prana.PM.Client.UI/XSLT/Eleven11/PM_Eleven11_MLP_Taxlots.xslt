<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<PositionMaster>
					<!--   Fund -->
					<FundName>
						<xsl:value-of select="''"/>
					</FundName>

					<xsl:variable name = "PB_COMPANY" >
						<xsl:value-of select="translate(COL2,'&quot;','')"/>
					</xsl:variable>
					<xsl:variable name="PRANA_SYMBOL">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MLP']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test ="$PRANA_SYMBOL != ''">
							<Symbol>
								<xsl:value-of select="$PRANA_SYMBOL"/>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="translate(COL2, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose >

					<PBSymbol>
						<xsl:value-of select="COL2"/>
					</PBSymbol>

					<!--<PBAssetType>
							<xsl:value-of select="COL27"/>
						</PBAssetType>-->
					<!-- Column 8 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
					<xsl:choose>
						<xsl:when test ="COL3 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL3*(-1)"/>
							</NetPosition>
						</xsl:when >
						<xsl:when test ="COL3 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL3"/>
							</NetPosition>
						</xsl:when >
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:choose>
						<xsl:when test ="COL1 = 'SS' or COL1 = 'ss' or COL1 = 'Short'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL1 = 'BUY' or COL1='Buy'">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL1 = 'SELL' or COL1 = 'Sell'">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:choose>
						<xsl:when test ="boolean(number(COL4))">
							<CostBasis>
								<xsl:value-of select="COL4"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>

					<!--<xsl:variable name = "varPBCounterParty" >
						<xsl:value-of select="translate(COL7, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
					</xsl:variable>-->

					<xsl:variable name = "PB_COUNTERPARTY" >
						<xsl:value-of select="translate(COL7, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
					</xsl:variable>
					<xsl:variable name="PRANA_COUNTERPARTYCODE">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='MLP']/BrokerData[@PranaBroker=$PB_COUNTERPARTY]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_COUNTERPARTYCODE != ''">
							<CounterPartyID>
								<xsl:value-of select="$PRANA_COUNTERPARTYCODE"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:otherwise>
							<CounterPartyID>
								<xsl:value-of select="'13'"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose>
					
					<!--<xsl:choose>
						<xsl:when test="$varPBCounterParty = 'UBS'">
							<CounterPartyID>
								<xsl:value-of select="'1'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'BARC'">
							<CounterPartyID>
								<xsl:value-of select="'2'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'MAXM'">
							<CounterPartyID>
								<xsl:value-of select="'16'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'LESW'">
							<CounterPartyID>
								<xsl:value-of select="'15'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'JONE'">
							<CounterPartyID>
								<xsl:value-of select="'12'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'CS'">
							<CounterPartyID>
								<xsl:value-of select="'0'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'NITE'">
							<CounterPartyID>
								<xsl:value-of select="'13'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'LQNT'">
							<CounterPartyID>
								<xsl:value-of select="'0'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'CANT'">
							<CounterPartyID>
								<xsl:value-of select="'7'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'COWN'">
							<CounterPartyID>
								<xsl:value-of select="'9'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'PULS'">
							<CounterPartyID>
								<xsl:value-of select="'18'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:otherwise>
							<CounterPartyID>
								<xsl:value-of select="'13'"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose >-->

					<PositionStartDate>
						<xsl:value-of select="''"/>
					</PositionStartDate>

				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
