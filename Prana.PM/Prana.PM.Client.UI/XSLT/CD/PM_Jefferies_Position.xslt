<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>



			<xsl:for-each select="//PositionMaster">

				<!--<xsl:variable name ="PBSymbol">
				<xsl:value-of select="normalize-space(substring(COL1,166,8))"/>
			  </xsl:variable>
			  
				<xsl:variable name ="PranaSymbol_MoneyMarketFund">
				<xsl:value-of select="document('../ReconMappingXml/MoneyMarketSymbolMapping.xml')/SymbolMapping/PB[@Name='Jefferies']/SymbolData[@FundSymbol=$PBSymbol]/@FundSymbol"/>
			  </xsl:variable>-->
				<xsl:variable name = "PB_FUND_NAME" >
					<xsl:value-of select="substring(COL1,10,8)"/>
				</xsl:variable>
					<xsl:if test="substring(COL1,59,13) != 0 and ($PB_FUND_NAME= '64980022' or $PB_FUND_NAME= '64980032')">
					<PositionMaster>
						<!--   Fund -->


						<!--<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>-->

						<AccountName>
							<xsl:value-of select="''"/>
						</AccountName>

						<xsl:variable name ="varCallPutIndicator">
							<xsl:value-of select ="normalize-space(substring(COL1,73,4))"/>
						</xsl:variable>

						<xsl:variable name="varStrikePrice">
							<xsl:choose>
								<xsl:when test ="$varCallPutIndicator='PUT' or $varCallPutIndicator='CALL'">
									<xsl:variable name ="varStrikePInt" select ="substring(COL1,94,5)"/>
									<xsl:variable name ="varInt" select ="translate($varStrikePInt,' ','0')"/>
									<xsl:variable name ="varStrikePDec" select ="substring(COL1,100,3)"/>
									<xsl:variable name ="varDec" select ="translate($varStrikePDec,' ','0')"/>
									<xsl:value-of select ="concat($varInt,$varDec)"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varCP">
							<xsl:choose>
								<xsl:when test ="$varCallPutIndicator='PUT'">									
									<xsl:value-of select ="'P'"/>
								</xsl:when >
								<xsl:when test ="$varCallPutIndicator='CALL'">
									<xsl:value-of select ="'C'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>




						<xsl:variable name ="OSI_Symbol">
							<xsl:value-of select="concat(substring(COL1,78,6),substring(COL1,91,2),substring(COL1,85,2),substring(COL1,88,2),$varCP,$varStrikePrice)"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="normalize-space(substring(COL1,166,8)) = '' and normalize-space(substring(COL1,182,9)) =''">

								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<CUSIP>
									<xsl:value-of select="''"/>
								</CUSIP>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							
								<xsl:when test ="$varCallPutIndicator='PUT' or $varCallPutIndicator='CALL'">

										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<CUSIP>
											<xsl:value-of select="''"/>
										</CUSIP>
										<IDCOOptionSymbol>
											<xsl:value-of select="concat($OSI_Symbol,'U')"/>
										</IDCOOptionSymbol>
									<PBSymbol>
										<xsl:value-of select="$OSI_Symbol"/>
									</PBSymbol>
									
									</xsl:when>

									<xsl:when test="normalize-space(substring(COL1,166,8)) = ''">
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<CUSIP>
											<xsl:value-of select="normalize-space(substring(COL1,182,9))"/>
										</CUSIP>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="normalize-space(substring(COL1,166,8))"/>
										</Symbol>
										<CUSIP>
											<xsl:value-of select="''"/>
										</CUSIP>
										<IDCOOptionSymbol>
											<xsl:value-of select="concat($OSI_Symbol,'U')"/>
										</IDCOOptionSymbol>
									</xsl:otherwise>
							</xsl:choose>

						



						<xsl:choose>
							<xsl:when test="number(substring(COL1,59,13)) and substring(COL1,58,1)='-' and ($varCallPutIndicator !='PUT' and $varCallPutIndicator !='CALL')">
								<NetPosition>
									<xsl:value-of select="substring(COL1,59,13)"/>
								</NetPosition>
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="number(substring(COL1,59,13)) and substring(COL1,58,1)=' ' and ($varCallPutIndicator !='PUT' and $varCallPutIndicator !='CALL')">
								<NetPosition>
									<xsl:value-of select="substring(COL1,59,13)"/>
								</NetPosition>
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="number(substring(COL1,59,13)) and substring(COL1,58,1)='-' and ($varCallPutIndicator ='PUT' or $varCallPutIndicator ='CALL')">
								<NetPosition>
									<xsl:value-of select="substring(COL1,59,13)"/>
								</NetPosition>
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="number(substring(COL1,59,13)) and substring(COL1,58,1)=' ' and ($varCallPutIndicator ='PUT' or $varCallPutIndicator ='CALL')">
								<NetPosition>
									<xsl:value-of select="substring(COL1,59,13)"/>
								</NetPosition>
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>


						<!--<NetPosition>
							<xsl:value-of select="10"/>
						</NetPosition>
						<SideTagValue>
							<xsl:value-of select="'5'"/>
						</SideTagValue>-->

						<PositionStartDate>
							<xsl:value-of select="concat(substring(COL1,1,2),'/',substring(COL1,4,2),'/','20',substring(COL1,7,2))"/>
						</PositionStartDate>



						<xsl:choose>
							<xsl:when test ="number(substring(COL1,192,11))">
								<CostBasis>
									<xsl:value-of select="substring(COL1,192,11)"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>

						</xsl:choose>

					</PositionMaster>
					</xsl:if>
				</xsl:for-each>
			
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>