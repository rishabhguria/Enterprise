<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<!-- Put = 0,Call = 1 , Here First call/put code then 2 characters for month code -->
		<!-- Call month Codes e.g. 101 represents 1=Call, 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth=101">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=102">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=103">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=104">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=105">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=106">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=107">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=108">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=109">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=110">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=111">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=112">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<!-- Put month Codes e.g. 001 represents 0=Put, 01 = January-->
			<xsl:when test ="$varMonth=001">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=002">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=003">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=004">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=005">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=006">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=007">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=008">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=009">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=010">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=011">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=012">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>



	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
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
						<xsl:variable name="varOptUnderlying" select="normalize-space(substring(COL1,131,12))" />
						<xsl:variable name="varOptExpyear" select="normalize-space(substring(COL1,639,2))" />
						<xsl:variable name="varOptExpMonth" select="normalize-space(substring(COL1,642,2))" />
						<xsl:variable name="Strike_Price" select="number(normalize-space(substring(COL1,649,18)))"/>
						
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
						
				<xsl:variable name = "varMonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="concat($varCallPutCode,$varOptExpMonth)" />
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
							<xsl:when test ="$varCallPutCode !=''">
								<Symbol>
									<xsl:value-of select="concat('O:',$varOptUnderlying,' ',$varOptExpyear,$varMonthCode,$varStrike)"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="$varPBSymbol"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>



						<PBSymbol>
							<xsl:value-of select="normalize-space(substring(COL1,131,12))"/>
						</PBSymbol>

						<PBAssetName>
							<xsl:value-of select="normalize-space(substring(COL1,119,6))"/>
						</PBAssetName>

						
			
					<xsl:choose>
						<xsl:when test="boolean(number(normalize-space(substring(COL1,861,18))))">
							<AvgPX>
								<xsl:value-of select="number(normalize-space(substring(COL1,861,18)))"/>
							</AvgPX>
						</xsl:when>
						<xsl:otherwise>
							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>
						</xsl:otherwise>
					</xsl:choose>
					
					<PositionStartDate>
						<xsl:value-of select="concat(substring(COL1,808,4),'/',substring(COL1,812,2),'/',substring(COL1,814,2))"/>
					</PositionStartDate>

						

					<xsl:choose>
						<xsl:when test="boolean(number(normalize-space(substring(COL1,786,19))))">
							<Quantity>
								<xsl:value-of select="number(normalize-space(substring(COL1,786,19)))"/>
							</Quantity>
						</xsl:when>
						<xsl:otherwise>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>
						</xsl:otherwise>
					</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(normalize-space(substring(COL1,986,12))))">
								<Commission>
									<xsl:value-of select="number(normalize-space(substring(COL1,986,12)))"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:variable name ="varSide">
							<xsl:value-of select="substring(COL1,835,1)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varSide = '1' and $varCallPutCode=''">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='2'and $varCallPutCode=''">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='5' and $varCallPutCode=''">
								<Side>
									<xsl:value-of select="'Short sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='BTC' and $varCallPutCode=''">
								<Side>
									<xsl:value-of select="'B'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide = '1' and $varCallPutCode != ''">
								<Side>
									<xsl:value-of select="'Buy to Open'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='2'and $varCallPutCode != ''">
								<Side>
									<xsl:value-of select="'Sell to Close'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='SS'and $varCallPutCode != ''">
								<Side>
									<xsl:value-of select="'C'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

				</PositionMaster>
			
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


