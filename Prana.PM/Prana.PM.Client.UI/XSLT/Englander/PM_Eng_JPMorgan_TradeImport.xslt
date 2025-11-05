<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth=01">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=02">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=03">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=04">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=05">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=06">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=07">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=08">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=09">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=10">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=13">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=14">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=15">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=16">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=17">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=18">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=19">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=20">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=21">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=22">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=23">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=24">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="substring(COL1,80,1)='T'">  
				<PositionMaster>
					<!--   Fund -->
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:if test ="substring(COL1,4,5)!= '-OF-F'">
							<xsl:value-of select="substring(COL1,4,5)"/>
						</xsl:if>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='JPMorgan']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

					<CUSIP>
						<xsl:value-of select="substring(COL1,11,12)"/>
					</CUSIP>


					<xsl:variable name="PB_COMPANY_NAME" select="substring(COL1,34,30)"/>
					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
					</xsl:variable>

					<Description>
						<xsl:value-of select ="$PB_COMPANY_NAME"/>
					</Description>

					<xsl:variable name ="varSymbol">
						<xsl:value-of select ="normalize-space(substring(COL1,23,8))"/>
					</xsl:variable>

					
					
					<xsl:variable name="varUnderlying" select="normalize-space(substring(COL1,219,8))"/>
					
					<xsl:variable name="varOptExpiration_Year" select="substring(COL1,52,2)" />
					<xsl:variable name="varOptExpiration_Month" select="substring(COL1,200,2)" />
					<xsl:variable name="Strike_PriceInt" select="number(substring(COL1,203,8))"/>
					<xsl:variable name="Strike_Price" select="concat($Strike_PriceInt,'.',substring(COL1,211,8))"/>

					<xsl:variable name ="varCallPutCode">
						<xsl:choose>
							<xsl:when test ="substring(COL1,200,2)='00'">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:when test ="number(substring(COL1,200,2)) &lt; 13 and number(substring(COL1,200,2)) &gt; 0">
								<xsl:value-of select ="'1'"/>
							</xsl:when>
							<xsl:when test ="number(substring(COL1,200,2)) &gt; 12">
								<xsl:value-of select ="'0'"/>
							</xsl:when>
							<xsl:when test ="substring(COL1,200,2)=0">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name = "varMonthCode" >
						<xsl:call-template name="MonthCode">
							<xsl:with-param name="varMonth" select="$varOptExpiration_Month" />
						</xsl:call-template>
					</xsl:variable>


					<xsl:variable name="varStrike">
						<xsl:choose>
							<xsl:when test="$varCallPutCode !=''">
								<xsl:variable name ="varStrikeDecimal" select ="substring-after($Strike_Price,'.')"/>
								<xsl:variable name ="varStrikeInt" select ="substring-before($Strike_Price,'.')"/>
								<xsl:choose>
									<xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
										<xsl:value-of select ="concat($Strike_Price,'0')"/>
									</xsl:when>
									<xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
										<xsl:value-of select ="$Strike_Price"/>
									</xsl:when>
									<xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
										<xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="concat($Strike_Price,'.00')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_SYMBOL_NAME != ''">
							<Symbol>
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</Symbol>
						</xsl:when>
						<xsl:when test ="$varCallPutCode !=''">
							<Symbol>
								<xsl:value-of select="concat('O:',$varUnderlying,' ',$varOptExpiration_Year,$varMonthCode,$varStrike)"/>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="$varSymbol"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose >


					<PBSymbol>
						<xsl:value-of select="substring(COL1,23,8)"/>

					</PBSymbol>

					<xsl:variable name ="varNetPosition">
						<xsl:value-of select ="substring(COL1,104,18)"/>
					</xsl:variable>

					<xsl:variable name ="varQtyInt">
						<xsl:value-of select ="substring($varNetPosition,1,13)"/>
					</xsl:variable>

					<xsl:variable name ="varQtyFrac">
						<xsl:value-of select ="substring($varNetPosition,14,5)"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="boolean(number($varNetPosition))">
							<NetPosition>
								<xsl:value-of select="concat($varQtyInt,'.',$varQtyFrac)"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>
					
					<xsl:choose>
						<xsl:when test="normalize-space(substring(COL1,64,8))!=''">
							<PositionStartDate>
								<xsl:value-of select="concat('20',substring(COL1,70,2),'/',substring(COL1,64,2),'/',substring(COL1,66,2))"/>
							</PositionStartDate>
						</xsl:when>
						<xsl:otherwise>
							<PositionStartDate>
								<xsl:value-of select="''"/>
							</PositionStartDate>
						</xsl:otherwise>
					</xsl:choose>

					
					<!--Side-->
					<xsl:variable name ="varSide">
						<xsl:value-of select="normalize-space(substring(COL1,630,4))"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$varSide='B' and $varCallPutCode =''">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="($varSide='S' or $varSide='SE')and $varCallPutCode =''">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide='SHS' and $varCallPutCode =''">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide='CVS' ">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when>

						<xsl:when test="$varSide='B' and $varCallPutCode !=''">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="($varSide='S' or $varSide='CO') and $varCallPutCode !=''">
							<SideTagValue>
								<xsl:value-of select="'D'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="($varSide='SHS' or $varSide='WO') and $varCallPutCode !=''">
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

					<xsl:variable name ="varPrice">
						<xsl:value-of select ="substring(COL1,86,17)"/>
					</xsl:variable>

					<xsl:variable name ="varPriceInt">
						<xsl:value-of select ="substring($varPrice,1,9)"/>
					</xsl:variable>

					<xsl:variable name ="varPriceFrac">
						<xsl:value-of select ="substring($varPrice,10,7)"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="boolean(number($varPrice))">
							<CostBasis>
								<xsl:value-of select="concat($varPriceInt,'.',$varPriceFrac)"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>

					
					<xsl:variable name ="varCommission">
						<xsl:value-of select ="substring(COL1,139,13)"/>
					</xsl:variable>

					<xsl:variable name ="varCommissionInt">
						<xsl:value-of select ="substring($varCommission,1,11)"/>
					</xsl:variable>

					<xsl:variable name ="varCommissionFrac">
						<xsl:value-of select ="substring($varCommission,12,2)"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="boolean(number($varCommission))">
							<Commission>
								<xsl:value-of select="concat($varCommissionInt,'.',$varCommissionFrac)"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>
				
					<!--security and exchange fee-->
					<xsl:variable name ="varFees">
						<xsl:value-of select ="substring(COL1,153,11)"/>
					</xsl:variable>

					<xsl:variable name ="varFeesInt">
						<xsl:value-of select ="substring($varFees,1,9)"/>
					</xsl:variable>

					<xsl:variable name ="varFeesFrac">
						<xsl:value-of select ="substring($varFees,10,2)"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="boolean(number($varFees))">
							<Fees>
								<xsl:value-of select="concat($varFeesInt,'.',$varFeesFrac)"/>
							</Fees>
						</xsl:when>
						<xsl:otherwise>
							<Fees>
								<xsl:value-of select="0"/>
							</Fees>
						</xsl:otherwise>
					</xsl:choose>


					<PBAssetType>
						<xsl:value-of select="''"/>
					</PBAssetType>

				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>