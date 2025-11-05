<?xml version="1.0" encoding="utf-8"?>

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

	<!-- for future month code ............F,G,H,J,K,M,N,Q,U,V,X,Z-->
	<xsl:template name="MonthCode_FutureOption">
		<xsl:param name="varMonth_FutureOption"/>
		<!-- 2 characters for month code -->
		<!-- month Codes e.g. 01 represents 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth_FutureOption=01">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=02">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=03">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=04">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=05">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=06">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=07">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=08">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=09">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=10">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=11">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth_FutureOption=12">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

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

				<xsl:if test ="COL1!='side' and COL1!='M'">
				<PositionMaster>
					<!--   Fund -->
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL13"/>
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

					<xsl:variable name ="varAssetClass" select ="normalize-space(COL26)"/>
					<xsl:variable name="varPBSymbol" select="COL2"/>
					
					<!--Uderlying for both EquityOption and FutureOption--> 
					
					<xsl:variable name ="varUnderlying">
						<xsl:choose>
							<xsl:when test ="contains(COL21,' ')!=False">
								<xsl:value-of select ="substring-before(COL21,' ')"/>
							</xsl:when>
							<xsl:when test ="contains(COL21,' ')=False">
								<xsl:value-of select ="normalize-space(COL21)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<!--Expiration Date-->
					<xsl:variable name="varExpirationDate" select="normalize-space(COL19)"/>
					<xsl:variable name ="varOptipn_Expirationyear">
						<xsl:choose>
							<xsl:when test ="string-length($varExpirationDate)=10">
								<xsl:value-of select ="substring($varExpirationDate,9,2)"/>
							</xsl:when>
							<xsl:when test ="string-length($varExpirationDate)=9">
								<xsl:value-of select ="substring($varExpirationDate,8,2)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<xsl:variable name="varOption_month" select="substring-before($varExpirationDate,'/')" />
					<xsl:variable name ="varOption_ExpirationMonth">
						<xsl:choose>
							<xsl:when test ="string-length($varOption_month)=1">
								<xsl:value-of select ="concat('0',$varOption_month)"/>
							</xsl:when>
							<xsl:when test ="string-length($varOption_month)=2">
								<xsl:value-of select ="$varOption_month"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					
					
						<!--Call/Put code for  month code for Equity Option-->
						<xsl:variable name ="varCallPutCode">
							<xsl:choose>
								<xsl:when test ="substring(normalize-space(COL20),1,1)='C' or substring(normalize-space(COL20),1,1)='c'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="substring(normalize-space(COL20),1,1)='P'or substring(normalize-space(COL20),1,1)='p'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
					
									
						<xsl:variable name = "varMonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="concat($varCallPutCode,$varOption_ExpirationMonth)" />
							</xsl:call-template>
						</xsl:variable>
						
					<!--Strike price for Equity Option-->
					
						<xsl:variable name="Strike_Price" select="COL18"/>
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

					<!-- For Creating ticker symbol for future Option-->
					
					<!--Underlying Symbol + Space 1 + Month Code 1 character + 1 digit year code + P or C + Strike price-->
					
					<xsl:variable name ="varFutureOption_Year">
						<xsl:choose>
							<xsl:when test ="string-length($varExpirationDate)=10">
								<xsl:value-of select ="substring($varExpirationDate,10,1)"/>
							</xsl:when>
							<xsl:when test ="string-length($varExpirationDate)=9">
								<xsl:value-of select ="substring($varExpirationDate,9,1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<xsl:variable name = "varMonthCode_FutureOption" >
						<xsl:call-template name="MonthCode_FutureOption">
							<xsl:with-param name="varMonth_FutureOption" select="$varOption_month" />
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name ="varCallPutSymbol">
						<xsl:choose>
							<xsl:when test ="substring(normalize-space(COL20),1,1)='C' or substring(normalize-space(COL20),1,1)='c'">
								<xsl:value-of select ="'C'"/>
							</xsl:when>
							<xsl:when test ="substring(normalize-space(COL20),1,1)='P'or substring(normalize-space(COL20),1,1)='p'">
								<xsl:value-of select ="'P'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<xsl:variable name ="varStrikeIntPart">
						<xsl:choose>
							<xsl:when test ="contains(COL18,'.')!= false">
								<xsl:value-of select ="substring-before(COL18,'.')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="COL18"/>
							</xsl:otherwise>
						</xsl:choose>
					
					</xsl:variable>
					<xsl:variable name ="varStrikeDecimalPart">
						<xsl:choose>
							<xsl:when test ="contains(COL18,'.')!= false">
								<xsl:value-of select ="substring-after(COL18,'.')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</xsl:variable>
					
					<xsl:variable name ="varFutureOption_StrikePrice">
						<xsl:choose>
							<xsl:when test ="contains(COL18,'.')!=False">
								<xsl:value-of select ="concat($varStrikeIntPart,$varStrikeDecimalPart)"/>
							</xsl:when>
							<xsl:when test ="contains(COL21,'.')=False">
								<xsl:value-of select ="$varStrikeIntPart"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					
					
					<xsl:choose>
						<xsl:when test ="$varAssetClass='EquityOption' and string-length($varPBSymbol)=21 and contains($varPBSymbol,'/')= false">
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="concat($varPBSymbol,'U')"/>
							</IDCOOptionSymbol>
						</xsl:when>
						<xsl:when test ="$varAssetClass='EquityOption' and string-length($varPBSymbol)=22 and  contains($varPBSymbol,'/')= false">
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="$varPBSymbol"/>
							</IDCOOptionSymbol>
						</xsl:when>
						<xsl:when test ="$varAssetClass='EquityOption' and contains($varPBSymbol,'O:')= false">
							<Symbol>
								<xsl:value-of select="concat('O:',$varUnderlying,' ',$varOptipn_Expirationyear,$varMonthCode,$varStrike)"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>
						</xsl:when>

						<xsl:when test ="$varAssetClass='FutureOption'">
							<Symbol>
								<xsl:value-of select="concat($varUnderlying,' ',$varMonthCode_FutureOption,$varFutureOption_Year,$varCallPutSymbol,$varFutureOption_StrikePrice)"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
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
					
								
					
					<PBSymbol>
						<xsl:value-of select ="COL2"/>
					</PBSymbol>

					
					<xsl:choose>
						<xsl:when test="COL9 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL9"/>
							</NetPosition>
						</xsl:when>
						<xsl:when test="COL9 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL9*(-1)"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>


				
					<xsl:variable name ="varSide" select="normalize-space(COL1)"/>
					<xsl:choose>
						<xsl:when test="$varSide = 'BO' and $varCallPutCode =''">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide='SC'and $varCallPutCode =''">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide='SS' and $varCallPutCode =''">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide='BC'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide = 'BO' and $varCallPutCode!=''">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide='SS'and $varCallPutCode!=''">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="$varSide='SC'and $varCallPutCode!=''">
							<SideTagValue>
								<xsl:value-of select="'D'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="COL10 = 'tradedate' or COL10='*'">
							<PositionStartDate>
								<xsl:value-of select="''"/>
							</PositionStartDate>
						</xsl:when>
						<xsl:otherwise>
							<PositionStartDate>
								<xsl:value-of select="COL10"/>
							</PositionStartDate>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="boolean(number(COL15))">
							<Commission>
								<xsl:value-of select="COL15"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="boolean(number(COL17))">
							<SecFees>
								<xsl:value-of select="COL17"/>
							</SecFees>
						</xsl:when>
						<xsl:otherwise>
							<SecFees>
								<xsl:value-of select="0"/>
							</SecFees>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="boolean(number(COL27))">
							<MiscFees>
								<xsl:value-of select="COL27"/>
							</MiscFees>
						</xsl:when>
						<xsl:otherwise>
							<MiscFees>
								<xsl:value-of select="0"/>
							</MiscFees>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test="boolean(number(COL11))">
							<CostBasis>
								<xsl:value-of select="COL11"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:variable name="PB_CounterParty" select="translate(COL12,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
					<xsl:variable name="PRANA_CounterPartyCode">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='ML']/BrokerData[translate(@MLBroker,$vLowercaseChars_CONST,$vUppercaseChars_CONST)=$PB_CounterParty]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="$PRANA_CounterPartyCode !=''">
							<CounterPartyID>
								<xsl:value-of select="$PRANA_CounterPartyCode"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:otherwise>
							<CounterPartyID>
								<xsl:value-of select="56"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose>
				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
	
	
</xsl:stylesheet>