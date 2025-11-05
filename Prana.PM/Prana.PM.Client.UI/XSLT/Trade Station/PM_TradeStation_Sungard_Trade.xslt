<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<!--<xsl:if test="substring(COL1,22,8)='19210939'or substring(COL1,22,8)='19210558'">-->
					<!--or substring(COL,22,8)='19210558'-->
					<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="substring(COL1,22,8)"/>
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

						<xsl:variable name="varPBSymbol" select="substring(COL1,65,12)"/>
						<xsl:variable name="varUnderlying" select="substring(COL1,445,6)"/>
						<xsl:variable name="varDateCallPutCode" select="substring(COL1,453,7)"/>

						<!--Code to know Call/Put Code-->
						<xsl:variable name ="varCallPutCode">
							<xsl:choose>
								<xsl:when test ="substring(COL1,459,1)='C'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="substring(COL1,459,1)='P'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="varStrike" select="normalize-space(substring(COL1,460,12))"/>
						<xsl:variable name ="varStrikeIntPart">
							<xsl:choose>
								<xsl:when test ="contains($varStrike,'.')!= false">
									<xsl:value-of select ="substring-before($varStrike,'.')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$varStrike"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varStrikeDecimalPart">
							<xsl:choose>
								<xsl:when test ="contains($varStrike,'.')!= false">
									<xsl:value-of select ="substring-after($varStrike,'.')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varStrikeInt">
							<xsl:choose>
								<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 1">
									<xsl:value-of select ="concat('0000',$varStrikeIntPart)"/>
								</xsl:when>
								<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 2">
									<xsl:value-of select ="concat('000',$varStrikeIntPart)"/>
								</xsl:when>
								<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 3">
									<xsl:value-of select ="concat('00',$varStrikeIntPart)"/>
								</xsl:when>
								<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 4">
									<xsl:value-of select ="concat('0',$varStrikeIntPart)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varStrikeIntPart"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varStrikeDecimal">
							<xsl:choose>
								<xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 1">
									<xsl:value-of select ="concat($varStrikeDecimalPart,'00')"/>
								</xsl:when>
								<xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 2">
									<xsl:value-of select ="concat($varStrikeDecimalPart,'0')"/>
								</xsl:when>
								<xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 3">
									<xsl:value-of select ="$varStrikeDecimalPart"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="substring($varStrikeDecimalPart,1,3)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Strike">
							<xsl:choose>
								<xsl:when test="$varCallPutCode !='' and contains($varStrike,'.')!= false">
									<xsl:value-of select ="concat($varStrikeInt,$varStrikeDecimal)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="concat($varStrikeInt,'000')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varCallPutCode!=''">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat($varUnderlying,$varDateCallPutCode,$Strike,'U')"/>
								</IDCOOptionSymbol>
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
									<xsl:value-of select ="substring(COL1,445,28)"/>
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<PBSymbol>
									<xsl:value-of select="substring(COL1,65,12)"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>
						
					
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
							<xsl:when test="substring(COL1,134,1) = 'B'and substring(COL1,30,1) != '3' and $varCallPutCode = ''">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="substring(COL1,134,1) = 'S' and substring(COL1,30,1) != '3' and $varCallPutCode = ''">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="substring(COL1,134,1) = 'S' and substring(COL1,30,1) = '3' and $varCallPutCode = ''">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="substring(COL1,134,1) = 'B' and substring(COL1,30,1) = '3' ">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test=" substring(COL1,134,1) = 'B'and substring(COL1,30,1) != '3' and $varCallPutCode != ''">
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="substring(COL1,134,1) = 'S' and substring(COL1,30,1) != '3' and $varCallPutCode != ''">
								<SideTagValue>
									<xsl:value-of select="'D'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test=" substring(COL1,134,1) = 'S' and substring(COL1,30,1) = '3' and $varCallPutCode != ''">
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
				<!--</xsl:if>-->
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


