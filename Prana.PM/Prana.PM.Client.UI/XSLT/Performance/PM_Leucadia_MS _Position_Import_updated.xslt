<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name ="MonthCodevar">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>

			<xsl:when test ="$varMonth='01' or $varMonth='1' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' or $varMonth='2' and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03'or $varMonth='3' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' or $varMonth='4' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05' or $varMonth='5' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' or $varMonth='6' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' or $varMonth='7' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' or $varMonth='8' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' or $varMonth='9' and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11' and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12' and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>

			<xsl:when test ="$varMonth='01' or $varMonth='1'and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' or $varMonth='2' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03' or $varMonth='3' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' or $varMonth='4' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05'or $varMonth='5' and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' or $varMonth='6' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' or $varMonth='7' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' or $varMonth='8' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' or $varMonth='9' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12' and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="varExDate"/>
		<xsl:if test="contains(COL50,'PUTL') or contains(COL50,'CALLL') or contains(COL44,'USD')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL14,' ')"/>
			</xsl:variable>
			
			<!--<xsl:variable name="UnderlyingSymbol">
				<xsl:choose>
					<xsl:when test="contains(COL24,'Index')">
						<xsl:value-of select = "substring-before(COL19,'.')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="COL19"/>
					</xsl:otherwise>
				</xsl:choose>				
			</xsl:variable>-->
			
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL86,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(normalize-space(COL86),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(COL86,'/'),'/'),3,2)"/>						
			</xsl:variable>

			<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(COL50,1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number((COL56),'#.00')"/>				
			</xsl:variable>
			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodevar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
					<xsl:with-param name="varPutCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
			</xsl:if>
	</xsl:template>


	
		<xsl:template match="/">
			<DocumentElement>

				<xsl:for-each select="//PositionMaster">
					<xsl:if test="number(COL27) and COL8!='USD'  and COL50!='CASH'  and COL51!='FX FORWARDS'">
						<PositionMaster>

							<xsl:variable name="varPB_Name">
								<xsl:value-of select="'MS'"/>
							</xsl:variable>

							<xsl:variable name = "PB_FUND_NAME">
								<xsl:value-of select="COL3"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>

							<AccountName>
								<xsl:choose>
									<xsl:when test="COL3 = 'ASIYA - TOPWATER MAYFLY FUND  LLC'">
										<xsl:value-of select="'Asiya'"/>
									</xsl:when>
									<xsl:when test ="$PRANA_FUND_NAME!=''">
										<xsl:value-of select ="$PRANA_FUND_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="$PB_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</AccountName>

							<xsl:variable name="varAvgPrice">
								<xsl:choose>
									<xsl:when test="contains(COL52,'FX')">
										<xsl:value-of select="COL30"/>
									</xsl:when>
									<xsl:when test ="COL30='0'">
										<xsl:value-of select ="COL40 div COL28"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL30"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:variable>

							<CostBasis>
								<xsl:choose>
									<xsl:when test ="number($varAvgPrice)" >
										<xsl:value-of select ="$varAvgPrice"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select ="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</CostBasis>

							<xsl:variable name = "PB_SYMBOL_NAME" >
								<xsl:choose>
									<xsl:when test="(contains(COL50, 'PUTL') or contains(COL50, 'CALLL') or COL50='FUTURE' or COL51='OTC OPTIONS')">
										<xsl:value-of select ="COL6"/>
								</xsl:when>
									<xsl:when test ="COL6='' and COL51='EQUITY SWAP'">
										<xsl:value-of select ="COL24"/>
									</xsl:when>
									<xsl:when test ="COL14='*' or COL14=''">
										<xsl:value-of select ="COL6"/>
									</xsl:when>
									<xsl:otherwise>
									<xsl:value-of select ="COL14"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PBSuffixCode">
							<xsl:choose>
								<xsl:when test="contains(COL44,'KRW') or contains(COL44,'MYR')">
									<xsl:value-of select = "substring-after(COL12,'.')"/>
								</xsl:when>

								<xsl:when test="contains(COL44,'CAD')">
									<xsl:value-of select = "substring-after(COL12,'.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select = "substring-after(COL14,' ')"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</xsl:variable>

						<!--<xsl:variable name="PB_ExchangeCODE">
							<xsl:value-of select="substring-after(substring-before(COL5, ' Equity'), ' ')"/>
						</xsl:variable>-->

						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBSuffixCode=$PBSuffixCode]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varOption">
							<xsl:choose>
								<xsl:when test="(contains(COL50,'PUTL') or contains(COL50,'CALLL')) and contains(COL45,'USD')">
									<xsl:call-template name ="Option">
										<xsl:with-param name ="Symbol" select ="COL19"/>
										<xsl:with-param name ="varExDate" select="COL55"/>
									</xsl:call-template>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Month">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(COL86,'/')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="ExpDate">
							<xsl:choose>

								<xsl:when test="string-length(substring-before(substring-after(COL86,'/'),'/'))=1">
									<xsl:value-of select="concat('0',substring-before(substring-after(COL86,'/'),'/'),$Month,substring-after(substring-after(COL86,'/'),'/'))"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(substring-after(COL86,'/'),'/'))=2">
									<xsl:value-of select="concat(substring-before(substring-after(COL86,'/'),'/'),$Month,substring-after(substring-after(COL86,'/'),'/'))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="(contains(COL50, 'PUTL') or contains(COL50, 'CALLL'))  and COL44='USD'">
									<xsl:value-of select="$varOption"/>
		
								</xsl:when>

								<xsl:when test="contains(COL52, 'FX')">
									<xsl:value-of select="concat(substring-before(COL6,'/'),'-',substring-before(substring-after(COL6,'/'),' '),' ',$ExpDate)"/>
								</xsl:when>


								<xsl:when test="$PBSuffixCode='UN' or $PBSuffixCode='US'">
									<xsl:value-of select="substring-before(COL14,' ')"/>
								</xsl:when>
								<xsl:when test="COL51='Futures'">
									<xsl:value-of select="concat(substring(COL14,1,2),' ',substring(COL14,3,2))"/>
								</xsl:when>
					

								<xsl:when test="COL44='CAD'">
									<xsl:value-of select="concat(substring-before(COL12,'.'),$PRANA_Exchange)"/>
								</xsl:when>
								
								<xsl:when test="COL44='HKD' and COL50='STOCK'">
									<xsl:value-of select="concat(substring-before(COL12,'.'),$PRANA_Exchange)"/>
								</xsl:when>

								<xsl:when test="COL44='TWD' and contains(COL12, '.TWO')">
									<xsl:value-of select="concat(substring-before(COL14,' '),'-GTS')"/>
								</xsl:when>

								<xsl:when test="$PRANA_Exchange!=''">
									<xsl:value-of select="concat(substring-before(COL14,' '),$PRANA_Exchange)"/>
								</xsl:when>

								<xsl:when test="COL51='Convertible Bond' or COL51='Non-Conv Corp'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>



							<ISIN>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="(contains(COL50, 'PUTL') or contains(COL50, 'CALLL'))  and COL44='USD'">
										<xsl:value-of select="''"/>

									</xsl:when>

									<xsl:when test="contains(COL52, 'FX')">
										<xsl:value-of select="''"/>
									</xsl:when>


									<xsl:when test="$PBSuffixCode='UN' or $PBSuffixCode='US'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="COL51='Futures'">
										<xsl:value-of select="''"/>
									</xsl:when>


									<xsl:when test="COL44='CAD'">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="COL44='HKD' and COL50='STOCK'">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="COL44='TWD' and contains(COL12, '.TWO')">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$PRANA_Exchange!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="COL51='Convertible Bond' or COL51='Non-Conv Corp'">
										<xsl:value-of select="COL10"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ISIN>


							<xsl:variable name="varQuantity">
							<xsl:value-of select="COL27"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*-1'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL27)"/>
						</xsl:variable>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="(contains(COL50,'PUTL') or contains(COL50,'CALLL')) and contains(COL45,'USD')">
									<xsl:choose>
										<xsl:when test ="number($varQuantity) &lt; 0">
											<xsl:value-of select ="'C'"/>
										</xsl:when>
										<xsl:when test ="number($varQuantity) &gt; 0">
											<xsl:value-of select ="'A'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="number($varQuantity) &lt; 0">
											<xsl:value-of select ="'5'"/>
										</xsl:when>
										<xsl:when test ="number($varQuantity) &gt; 0">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="COL44"/>
						</PBSymbol>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="normalize-space(COL51)='EQUITY SWAP'">
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="translate(concat($PRANA_SYMBOL_NAME,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>

										<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
											<xsl:value-of select="translate(concat($varOption,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>

										<xsl:when test="$PBSuffixCode='UN' or $PBSuffixCode='US'">
											<xsl:value-of select="translate(concat(substring-before(COL14,' '),'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>

										<xsl:when test="$PRANA_Exchange!=''">
											<xsl:value-of select="translate(concat(concat(substring-before(COL14,' '),$PRANA_Exchange),'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="translate(concat($PB_SYMBOL_NAME,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										</xsl:when>

										<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
											<xsl:value-of select="$varOption"/>
										</xsl:when>

										<xsl:when test="$PBSuffixCode='UN' or $PBSuffixCode='US'">
											<xsl:value-of select="substring-before(COL14,' ')"/>
										</xsl:when>

										<xsl:when test="$PRANA_Exchange!=''">
											<xsl:value-of select="concat(substring-before(COL14,' '),$PRANA_Exchange)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</TradeAttribute1>


						<!--<xsl:variable name="varCommision">
							<xsl:value-of select="COL50"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($varCommision) &lt; 0'>
									<xsl:value-of select ='$varCommision*-1'/>
								</xsl:when>

								<xsl:when test ='number($varCommision) &gt; 0'>
									<xsl:value-of select ='$varCommision'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varStamp">
							<xsl:value-of select="COL52"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($varStamp) &lt; 0'>
									<xsl:value-of select ='$varStamp*-1'/>
								</xsl:when>

								<xsl:when test ='number($varStamp) &gt; 0'>
									<xsl:value-of select ='$varStamp'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>-->


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

