<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name = "PB_Fund" >
					<xsl:value-of select="COL6"/>
				</xsl:variable>
				<!--<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>-->
				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_Fund]/@PranaFund"/>
				</xsl:variable>


				<xsl:if test="COL8!='1'">
					<!--((COL7 = '0' and COL26 !='OA') or  (COL7 ='0' and COL26!= 'OE'))">-->
					<!--<xsl:if test ="substring(COL1,6,1) !='A' and substring(COL1,6,1) !='Z'">-->
					<!--<xsl:if test ="substring(COL1,6,1) !='A' and substring(COL1,6,1) !='Z'">-->

					<PositionMaster>
						<!--<xsl:variable name="PB_Fund" select="COL6"/>-->


						<AccountName>
							<xsl:value-of select="$PRANA_FUND_NAME"/>
						</AccountName>


						<xsl:variable name="varPBSymbol" select="COL2"/>
						<xsl:variable name="varSecurityType" select="COL7"/>

						<xsl:choose>
							<xsl:when test ="$varSecurityType='B' or $varSecurityType='J'">
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(substring(COL2,1,21),'U')"/>
								</IDCOOptionSymbol>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select ="COL2"/>
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL2"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								<PBSymbol>
									<xsl:value-of select ="COL2"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>


						<!--<PBSymbol>
							<xsl:value-of select ="COL2"/>
						</PBSymbol>-->
						<!--this is for in N order side  -->
						<xsl:variable name="Cost">
							<xsl:choose>
								<xsl:when test="number(COL1)">
									<xsl:value-of select="COL9 div COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:choose>
							<xsl:when test ="boolean(number($Cost))">
								<CostBasis>
									<xsl:value-of select="$Cost"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:variable name ="Position" select ="number(COL1)"/>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when >
								<xsl:when test ="$Position &lt; 0">
									<xsl:value-of select ="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</NetPosition>
						<!-- End This is for in O Quantity  -->
						<!-- This is for in  R Commission   -->
						<!--<xsl:choose>
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
						-->
						<!-- End This is for in  R Commission   --><!--
						<xsl:choose>
							<xsl:when test="boolean(number(COL21))">
								<MiscFees>
									<xsl:value-of select="COL21"/>
								</MiscFees>
							</xsl:when>
							<xsl:otherwise>
								<MiscFees>
									<xsl:value-of select="0"/>
								</MiscFees>
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
						</xsl:choose>-->
						<!--This is for in M order side-->
						
						
						<SideTagValue>

							<xsl:choose>

								<xsl:when test="(COL1 &lt; 0) and normalize-space(COL3='Stock - Common')">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="(COL1 &gt; 0) and normalize-space(COL3='Stock - Common')">
									<xsl:value-of select="'1'"/>
								</xsl:when>

							

								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>		

						<xsl:variable name = "PB_Broker" >
							<xsl:value-of select="normalize-space(COL23)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_Broker">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='SUNGARD']/BrokerData[@PBBroker=$PB_Broker]/@PranaBrokerCode"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="COL26 != 'ID' and COL26 != 'TH'">
								<CounterPartyID>
									<xsl:value-of select="16"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select="0"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>
						<PositionStartDate>
						<xsl:value-of select="COL6"/>
					</PositionStartDate>

						<OriginalPurchaseDate>
							<xsl:value-of select="COL15"/>
						</OriginalPurchaseDate>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


