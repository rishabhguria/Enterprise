<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<!--<xsl:if test="substring(COL1,22,8)='19210558'">-->
					<!--or substring(COL,22,8)='19210558'-->
				<xsl:if test ="substring(COL1,6,1) !='A' and substring(COL1,6,1) !='Z'">
					
			
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
									<xsl:value-of select="concat(substring(COL1,2601,21),'U')"/>
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
									<xsl:value-of select ="substring(COL1,2601,21)"/>
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<PBSymbol>
									<xsl:value-of select="normalize-space(substring(COL1,131,12))"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							
							
							<xsl:when  test="number(normalize-space(substring(COL1,861,18)))">
								<CostBasis>
									<xsl:value-of select="normalize-space(substring(COL1,861,18))"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >


						<xsl:variable name ="varDate">
							<xsl:value-of select ="concat(substring(COL1,808,4),'/',substring(COL1,812,2),'/',substring(COL1,814,2))"/>
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
							<xsl:when  test="number(normalize-space(substring(COL1,786,19)))">
								<NetPosition>
									<xsl:value-of select="normalize-space(substring(COL1,786,19))"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(normalize-space(substring(COL1,986,12))))">
								<Commission>
									<xsl:value-of select="normalize-space(substring(COL1,986,12))"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name="varClearingCharge1" select ="normalize-space(substring(COL1,938,12))" />
						<xsl:variable name="varClearingCharge2" select ="normalize-space(substring(COL1,955,12))" />
						<xsl:choose>
							<xsl:when test="$varClearingCharge1!=0 or $varClearingCharge2!=0">
								<ClearingFee>
									<xsl:value-of select="$varClearingCharge1 + $varClearingCharge2 "/>
								</ClearingFee>
							</xsl:when>
							<xsl:otherwise>
								<ClearingFee>
									<xsl:value-of select="0"/>
								</ClearingFee>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="varRegulatoryFee1" select ="number(normalize-space(substring(COL1,1071,12)))" />
						<xsl:variable name="varRegulatoryFee2" select ="number(normalize-space(substring(COL1,1088,12)))" />
						<xsl:variable name="varRegulatoryFee3" select ="number(normalize-space(substring(COL1,1105,12)))" />
						<xsl:variable name="varRebateFee1" select ="number(normalize-space(substring(COL1,1122,12)))" />
						<xsl:variable name="varRebateFee2" select ="number(normalize-space(substring(COL1,1139,12)))" />
						<xsl:choose>
							<xsl:when test="$varRegulatoryFee1!=0 or $varRegulatoryFee2!=0 or $varRegulatoryFee3!=0 or $varRebateFee1!=0 or $varRebateFee1!=0" >
								<MiscFees>
									<xsl:value-of select="$varRegulatoryFee1 + $varRegulatoryFee2 + $varRegulatoryFee3 + $varRebateFee1 + $varRebateFee1"/>
								</MiscFees>
							</xsl:when>
							<xsl:otherwise>
								<MiscFees>
									<xsl:value-of select="0"/>
								</MiscFees>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="varExchangeFee1" select ="number(normalize-space(substring(COL1,1003,12)))" />
						<xsl:variable name="varExchangeFee2" select ="number(normalize-space(substring(COL1,1020,12)))" />
						<xsl:variable name="varExchangeFee3" select ="number(normalize-space(substring(COL1,1037,12)))" />
						<xsl:variable name="varExchangeFee4" select ="number(normalize-space(substring(COL1,1054,12)))" />
						
						
							
								<StampDuty>
									<xsl:value-of select="$varExchangeFee1 + $varExchangeFee2 + $varExchangeFee3 + $varExchangeFee4"/>
								</StampDuty>
							
						
						<xsl:choose>
							<xsl:when test="boolean(number(normalize-space(substring(COL1,969,12))))">
								<Fees>
									<xsl:value-of select="number(normalize-space(substring(COL1,969,12)))"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select="0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name ="varSide">
							<xsl:value-of select="substring(COL1,835,1)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varSide = '1' and $varCallPutCode=''">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='2'and $varCallPutCode=''">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='5' and $varCallPutCode=''">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='BTC' and $varCallPutCode=''">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide = '1' and $varCallPutCode != ''">
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='2'and $varCallPutCode != ''">
								<SideTagValue>
									<xsl:value-of select="'D'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SS'and $varCallPutCode != ''">
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
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


