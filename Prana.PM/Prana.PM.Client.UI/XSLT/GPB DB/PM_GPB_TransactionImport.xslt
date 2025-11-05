<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="(COL19 ='PS' or COL19 ='EQ') and COL18='Y'">
				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL3"/>
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

						<xsl:variable name ="varPBSymbol">
							<xsl:choose>
								<xsl:when test ="COL19='PS'and contains(COL22,'_')!= false">
									<xsl:value-of select ="substring-before(COL22,'_')"/>
								</xsl:when>
								<xsl:when test ="COL19='PS' and contains(COL22,'_')= false">
									<xsl:value-of select ="COL22"/>
								</xsl:when>
								<xsl:when test ="COL19='EQ'and COL20!='Miscellaneous'">
									<xsl:value-of select ="COL22"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL24"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varOptionString" select="COL24"/>
						<xsl:variable name="varOptionString_length" select="string-length($varOptionString)"/>
						<xsl:variable name="varRoot" select="substring($varOptionString,1,$varOptionString_length -15)"/>
						<xsl:variable name="varRemainingString" select="substring($varOptionString,$varOptionString_length -14)"/>
						
						<xsl:variable name ="varCallPutCode">
							<xsl:choose>
								<xsl:when test ="substring(COL24,$varOptionString_length -8,1)='C'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="substring(COL24,$varOptionString_length -8,1)='P'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "BlankCount_Root" >
							<xsl:call-template name="noofBlanks">
								<xsl:with-param name="count1" select="(6) - string-length($varRoot)" />
							</xsl:call-template>
						</xsl:variable>


					<xsl:choose>
						<xsl:when test ="$varCallPutCode !=''">
							<IDCOOptionSymbol>
								<xsl:value-of select="concat($varRoot,$BlankCount_Root,$varRemainingString,'U')"/>
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
									<xsl:value-of select ="COL24"/>
								</PBSymbol>
												
							</xsl:when>
							<xsl:otherwise>
								<PBSymbol>
									<xsl:value-of select="COL22"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when  test="number(COL33)">
								<CostBasis>
									<xsl:value-of select="COL33"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >
							
						<xsl:variable name ="varDate">
							<xsl:value-of select ="concat(substring(COL10,1,4),'/',substring(COL10,5,2),'/',substring(COL10,7,2))"/>
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
							<xsl:when  test="boolean(number(COL32)) and number(COL32) &lt; 0 ">
								<NetPosition>
									<xsl:value-of select="number(COL32)* -1"/>
								</NetPosition>
							</xsl:when>
							<xsl:when  test="boolean(number(COL32)) and number(COL32) &gt; 0">
								<NetPosition>
									<xsl:value-of select="number(COL32)"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL40))">
								<Commission>
									<xsl:value-of select="COL40"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:choose>
							<xsl:when test="boolean(number(COL41))">
								<Fees>
									<xsl:value-of select="number(COL41)"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select="0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name ="varSide">
							<xsl:value-of select="COL16"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varSide = 'BY' and $varCallPutCode=''">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SL'and $varCallPutCode=''">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SS' and $varCallPutCode=''">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='BC'">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide = 'BY' and $varCallPutCode != ''">
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SL'and $varCallPutCode != ''">
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
							<xsl:value-of select="COL50"/>
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


