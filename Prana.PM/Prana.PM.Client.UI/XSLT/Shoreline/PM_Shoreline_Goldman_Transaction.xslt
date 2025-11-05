<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">

				<xsl:if test ="COL9 != 'TradeDate'">
					<PositionMaster>

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Shoreline_Goldman']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>


						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when test="COL13 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL13*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL13 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL13"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varCommission">
							<xsl:choose>
								<xsl:when test="COL16 &lt; 0">
									<xsl:value-of select ="COL16 * (-1)"/>
								</xsl:when >
								<xsl:when test="COL16 &gt; 0">
									<xsl:value-of select ="COL16"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Commission>
							<xsl:value-of select="$varCommission"/>
						</Commission>

						<xsl:variable name ="varNetAmount">
							<xsl:choose>
								<xsl:when test="COL17 &lt; 0">
									<xsl:value-of select ="COL17 * (-1)"/>
								</xsl:when >
								<xsl:when test="COL17 &gt; 0">
									<xsl:value-of select ="COL17"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varQty">
							<xsl:choose>
								<xsl:when test="COL13 &lt; 0">
									<xsl:value-of select ="COL13 * (-1)"/>
								</xsl:when >
								<xsl:when test="COL13 &gt; 0">
									<xsl:value-of select ="COL13"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--<xsl:choose>
							<xsl:when test ="$varNetAmount &gt; 0 and $varQty &gt; 0">
								<CostBasis>
									<xsl:value-of select="($varNetAmount + $varCommission) div $varQty"/>
								</CostBasis>
							</xsl:when>							
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>-->

						<xsl:choose>
							<xsl:when test="COL13 &lt; 0">
								<xsl:choose>
									<xsl:when test ="$varNetAmount &gt; 0 and $varQty &gt; 0">
										<CostBasis>
											<xsl:value-of select="($varNetAmount + $varCommission) div $varQty"/>
										</CostBasis>
									</xsl:when>
									<xsl:otherwise>
										<CostBasis>
											<xsl:value-of select="0"/>
										</CostBasis>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when >
							<xsl:when test="COL13 &gt; 0 and number(COL14)">
								<CostBasis>
									<xsl:value-of select="COL14"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Position Date mapped with the column 9 -->
						<PositionStartDate>
							<xsl:value-of select="translate(COL9,'&quot;','')"/>
						</PositionStartDate>
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Shoreline_Goldman']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="COL8 != '' and COL8 != '*'">
								<Symbol>
									<xsl:value-of select="COL8"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
						<PBSymbol>
							<xsl:value-of select="translate(COL8,'&quot;','')"/>
						</PBSymbol>
						<PBAssetType>
							<xsl:value-of select="'EQUITY'"/>
						</PBAssetType>

						<xsl:variable name="varPBCounterParty">
							<xsl:value-of select ="substring-before(COL18,' ')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_COUNTERPARTYID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='Shoreline_Goldman']/BrokerData[@PBBroker=$varPBCounterParty]/@PranaBrokerID"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_COUNTERPARTYID != ''">
								<CounterPartyID>
									<xsl:value-of select="$PRANA_COUNTERPARTYID"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select="-1"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>