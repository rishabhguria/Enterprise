<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="temp_MonthExpireCode">
		<xsl:param name="param_MonthExpireCode"/>
		<xsl:choose>
			<xsl:when test ="$param_MonthExpireCode='01'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='02'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='03'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='04'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='05'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='06'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='07'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='08'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='09'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='10'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='11'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='12'">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="$param_MonthExpireCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name ="MonthCode">
		<xsl:param name="month"/>
		<xsl:choose>
			<xsl:when test="$month='Jan'">
				<xsl:value-of select ="'01'"/>
			</xsl:when>
			<xsl:when test="$month='Feb'">
				<xsl:value-of select ="'02'"/>
			</xsl:when>
			<xsl:when test="$month='Mar'">
				<xsl:value-of select ="'03'"/>
			</xsl:when>
			<xsl:when test="$month='Apr'">
				<xsl:value-of select ="'04'"/>
			</xsl:when>
			<xsl:when test="$month='May'">
				<xsl:value-of select ="'05'"/>
			</xsl:when>
			<xsl:when test="$month='Jun'">
				<xsl:value-of select ="'06'"/>
			</xsl:when>
			<xsl:when test="$month='Jul'">
				<xsl:value-of select ="'07'"/>
			</xsl:when>
			<xsl:when test="$month='Aug'">
				<xsl:value-of select ="'08'"/>
			</xsl:when>
			<xsl:when test="$month='Sep'">
				<xsl:value-of select ="'09'"/>
			</xsl:when>
			<xsl:when test="$month='Oct'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test="$month='Nov'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test="$month='Dec'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>

		</xsl:choose>
	</xsl:template>

	<xsl:template name="tempCurrencyCode">
		<xsl:param name="paramCurrencySymbol"/>
		<!-- 1 characters for metal code -->
		<!--  e.g. A represents A = aluminium-->
		<xsl:choose>
			<xsl:when test ="$paramCurrencySymbol='USD'">
				<xsl:value-of select ="'1'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='HKD'">
				<xsl:value-of select ="'2'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='JPY'">
				<xsl:value-of select ="'3'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GBP'">
				<xsl:value-of select ="'4'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='AED'">
				<xsl:value-of select ="'5'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BRL'">
				<xsl:value-of select ="'6'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CAD'">
				<xsl:value-of select ="'7'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='EUR'">
				<xsl:value-of select ="'8'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='NOK'">
				<xsl:value-of select ="'9'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SGD'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MUL'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ZAR'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SEK'">
				<xsl:value-of select ="'13'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='AUD'">
				<xsl:value-of select ="'14'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CNY'">
				<xsl:value-of select ="'15'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='KRW'">
				<xsl:value-of select ="'16'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BDT'">
				<xsl:value-of select ="'17'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='THB'">
				<xsl:value-of select ="'18'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='dong'">
				<xsl:value-of select ="'19'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GBX'">
				<xsl:value-of select ="'20'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='INR'">
				<xsl:value-of select ="'21'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CHF'">
				<xsl:value-of select ="'23'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CLP'">
				<xsl:value-of select ="'24'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='COP'">
				<xsl:value-of select ="'25'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CZK'">
				<xsl:value-of select ="'26'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='DKK'">
				<xsl:value-of select ="'27'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GHS'">
				<xsl:value-of select ="'28'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='HUF'">
				<xsl:value-of select ="'29'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='IDR'">
				<xsl:value-of select ="'30'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ILS'">
				<xsl:value-of select ="'31'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ISK'">
				<xsl:value-of select ="'32'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='KZT'">
				<xsl:value-of select ="'33'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='LVL'">
				<xsl:value-of select ="'34'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MXN'">
				<xsl:value-of select ="'35'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='NZD'">
				<xsl:value-of select ="'36'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PEN'">
				<xsl:value-of select ="'37'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PLN'">
				<xsl:value-of select ="'38'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='RON'">
				<xsl:value-of select ="'40'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='RUB'">
				<xsl:value-of select ="'41'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SKK'">
				<xsl:value-of select ="'42'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='TRY'">
				<xsl:value-of select ="'43'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ARS'">
				<xsl:value-of select ="'44'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='UYU'">
				<xsl:value-of select ="'45'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='TWD'">
				<xsl:value-of select ="'46'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BMD'">
				<xsl:value-of select ="'47'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='EEK'">
				<xsl:value-of select ="'48'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GEL'">
				<xsl:value-of select ="'49'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MYR'">
				<xsl:value-of select ="'51'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SIT'">
				<xsl:value-of select ="'52'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='XAF'">
				<xsl:value-of select ="'53'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='XOF'">
				<xsl:value-of select ="'54'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='AZN'">
				<xsl:value-of select ="'55'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PKR'">
				<xsl:value-of select ="'56'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PHP'">
				<xsl:value-of select ="'57'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">


				<xsl:variable name = "PB_FUND_NAME" >
					<xsl:value-of select="(COL1)"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CustomHouse']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:if test="COL1 != 'Custom_Account' and $PRANA_FUND_NAME !=''">

					<PositionMaster>
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME !=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>


						<xsl:variable name="varException">
							<xsl:choose>
								<xsl:when test="contains(COL28,'XXXXXXX')!= false">
									<xsl:value-of select="'XXXXXXX'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="varMonthExpireCode">


							<xsl:call-template name="temp_MonthExpireCode">
								<xsl:with-param name="param_MonthExpireCode" select="number(COL26)"/>
							</xsl:call-template>

						</xsl:variable>

						<xsl:variable name="varYearExpireCode" select="substring(COL27,4,1)"/>

						<xsl:variable name="varBBCode">
							<xsl:choose>
								<xsl:when test="COL28='' or COL28='*'">
									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space(COL76),' '))=1">
											<xsl:value-of select="substring-before(normalize-space(COL76),' ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(substring-before(normalize-space(COL76),' '),1,string-length(substring-before(normalize-space(COL76),' '))-2)"/>
										</xsl:otherwise>
									</xsl:choose>
									
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space(COL76),' '))=1">
											<xsl:value-of select="substring-before(normalize-space(COL76),' ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(substring-before(normalize-space(COL76),' '),1,string-length(substring-before(normalize-space(COL76),' '))-3)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
								

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varBBKey">

							<xsl:choose>
								<xsl:when test="COL28='' or COL28='*'">

									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space(COL76),' '))=1">
											<xsl:value-of select="substring-after(substring-after(normalize-space(COL76),' '),' ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-after(normalize-space(COL76),' ')"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space(COL76),' '))=1">
											<xsl:value-of select="substring-after(substring-after(substring-after(normalize-space(COL76),' '),' '),' ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-after(substring-after(normalize-space(COL76),' '),' ')"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@UnderlyingCode"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varStrikeMul">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@StrikeMul"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@StrikeMul"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExpFlag">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@ExpFlag"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExpFlag"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMulFlag">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@MulFlag"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@MulFlag"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExchangeCode">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@ExchangeCode"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="contains(COL32,'LME')!= false">
								<Symbol>
									<xsl:value-of select="concat($varUnderlying,' ',$varYearExpireCode,$varMonthExpireCode,substring(COL32,1,2),$varExchangeCode)"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="COL28='' or COL28='*'">
								<Symbol>
									<xsl:choose>
										<xsl:when test="$varExpFlag = '1'">
											<xsl:value-of select="concat($varUnderlying,' ',$varYearExpireCode,$varMonthExpireCode,$varExchangeCode)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat($varUnderlying,' ',$varMonthExpireCode,$varYearExpireCode,$varExchangeCode)"/>
										</xsl:otherwise>
									</xsl:choose>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="varStrikePart">
									<xsl:value-of select="format-number(COL29,'#.00') * format-number($varStrikeMul,'#.00')"/>
								</xsl:variable>
								<Symbol>
									<xsl:choose>
										<xsl:when test="$varExpFlag = '1'">
											<xsl:value-of select="concat($varUnderlying,' ',$varYearExpireCode,$varMonthExpireCode,substring(normalize-space(COL28),1,1),$varStrikePart,$varExchangeCode)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat($varUnderlying,' ',$varMonthExpireCode,$varYearExpireCode,substring(normalize-space(COL28),1,1),$varStrikePart,$varExchangeCode)"/>
										</xsl:otherwise>
									</xsl:choose>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>


						<PBSymbol>
							<xsl:value-of select="concat(COL32,'-',COL76,'-',$varBBCode,'-',$varBBKey,'-',$varUnderlying)"/>
						</PBSymbol>
						<!--<PBSymbol>
					  <xsl:value-of select="substring(substring-before(normalize-space(COL76),' '),1,string-length(substring-before(normalize-space(COL76),' '))-2)"/>
				  </PBSymbol>-->


						<AvgPx>
							<xsl:choose>
								<xsl:when  test="number(COL30) and COL30 &gt; 0 and $varMulFlag != 0 and $varMulFlag != ''">
									<xsl:value-of select="COL30 div $varMulFlag"/>
								</xsl:when >
								<xsl:when test="number(COL30) and COL30 &lt; 0 and $varMulFlag != 0 and $varMulFlag != ''">
									<xsl:value-of select="(COL30 * (-1)) div $varMulFlag"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</AvgPx>


						<!--<PositionStartDate>
              <xsl:choose>
                <xsl:when test ="COL14 !='' and COL14 != '*'">
                  <xsl:value-of select="COL14"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </PositionStartDate>-->

						<Date>
							<xsl:choose>
								<xsl:when test ="COL14 !='' and COL14 != '*'">
									<xsl:variable name="dd" select="substring(COL14,1,2)"/>
									<xsl:variable name="yy" select ="substring(COL14,7,4)"/>
									<xsl:variable name="mm">
										<xsl:call-template name="MonthCode">
											<xsl:with-param name="month" select="substring(COL14,3,3)"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select ="concat($mm,'/',$dd,'/',$yy)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Date>






						<Quantity>
							<xsl:choose>
								<xsl:when  test="COL4 !=''">
									<xsl:value-of select="number(COL4)"/>
								</xsl:when>
								<xsl:when  test="COL5 != ''">
									<xsl:value-of select="number(COL5)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<!--<MiscFees>
					  <xsl:choose>
						  <xsl:when  test="number(COL12) &gt; 0">
							  <xsl:value-of select="number(COL12)"/>
						  </xsl:when >
						  <xsl:when test="number(COL12) &lt; 0">
							  <xsl:value-of select="number(COL12) * (-1)"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="0"/>
						  </xsl:otherwise>
					  </xsl:choose >
				  </MiscFees>-->

						<Side>
							<xsl:choose>
								<xsl:when  test="COL19 ='1'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when  test="COL19 ='2'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						





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


