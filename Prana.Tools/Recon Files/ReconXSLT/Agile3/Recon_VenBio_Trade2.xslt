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

	<xsl:template name="MonthNo">
		<xsl:param name="varMonth"/>
		

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth='JAN' ">
				<xsl:value-of select ="'01'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='FEB'">
				<xsl:value-of select ="'02'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='MAR' ">
				<xsl:value-of select ="'03'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='APR'">
				<xsl:value-of select ="'04'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='MAY'">
				<xsl:value-of select ="'05'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='JUN' ">
				<xsl:value-of select ="'06'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='JUL'">
				<xsl:value-of select ="'07'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='AUG' ">
				<xsl:value-of select ="'08'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='SEP' ">
				<xsl:value-of select ="'09'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='OCT'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='NOV'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='DEC' ">
				<xsl:value-of select ="'12'"/>
			</xsl:when>		
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:param name="varPutCall"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="($varMonth='JAN' or $varMonth='01') and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='FEB' or $varMonth='02')  and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='MAR' or $varMonth='03') and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='APR' or $varMonth='04') and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='MAY' or $varMonth='05') and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='JUN' or $varMonth='06') and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='JUL' or $varMonth='07') and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='AUG' or $varMonth='08') and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='SEP' or $varMonth='09') and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='OCT' or $varMonth='10') and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='NOV' or $varMonth='11') and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='DEC' or $varMonth='12') and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='JAN' or $varMonth='01') and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='FEB' or $varMonth='02') and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='MAR' or $varMonth='03') and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='APR' or $varMonth='04') and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='MAY' or $varMonth='05') and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='JUN' or $varMonth='06') and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='JUL' or $varMonth='07') and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='AUG' or $varMonth='08') and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='SEP' or $varMonth='09') and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='OCT' or $varMonth='10') and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='NOV' or $varMonth='11') and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="($varMonth='DEC' or $varMonth='12') and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	

	<xsl:template name="Option">
		<xsl:param name="varSymbol"/>	


		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test="COL6='Option'">
					<xsl:value-of select ="substring-before($varSymbol,' ')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test="COL6='Option'">
					<xsl:choose>
						<xsl:when test="number(substring-before(substring-after($varSymbol,'/'),'/'))">
							<xsl:value-of select="substring(substring-after(substring-after($varSymbol,'/'),'/'),1,2)"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select ="substring(normalize-space(substring-after(substring-after($varSymbol,' '),' ')),1,2)"/>
						</xsl:otherwise>
					</xsl:choose>
					
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonthName">
			<xsl:choose>
				<xsl:when test="COL6='Option'">
					<xsl:choose>
						<xsl:when test="number(substring-before(substring-after($varSymbol,'/'),'/'))">
							<xsl:value-of select="substring(substring-after(substring-after($varSymbol,' '),' '),1,2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="normalize-space(substring-before(substring-after($varSymbol,'/'),'/'))"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test="COL6='Option'">
					<xsl:choose>
						<xsl:when test="number(substring-before(substring-after($varSymbol,'/'),'/'))">
							<xsl:value-of select="substring-before(substring-after($varSymbol,'/'),'/')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="normalize-space(substring-before(substring-after(substring-after($varSymbol,'/'),'/'),' '))"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test="COL6='Option'">
					<xsl:value-of select ="substring(normalize-space(substring-after(substring-after(substring-after($varSymbol,' '),' '),' ')),1,1)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		

		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test="COL6='Option'">
					<xsl:value-of select ="format-number(substring(normalize-space(substring-after(substring-after(substring-after($varSymbol,' '),' '),' ')),2),'#.00')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:call-template name ="MonthCode">
				<xsl:with-param name ="varMonth" select ="$varMonthName"/>
				<xsl:with-param name ="varPutCall" select="$varPutCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varDays">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)='0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when  test="number(varMonthName)">
					<xsl:value-of select="$varMonthName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="MonthNo">
						<xsl:with-param name="varMonth" select="$varMonthName"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
			
		</xsl:variable>

		<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="COL6='Option' and number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>

		<!--<xsl:value-of select="concat()"/>-->


		<xsl:choose>
			<xsl:when test="COL6='Option'">
				<xsl:choose>
					<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
		
	</xsl:template>

	<xsl:template match="/">

		<NewDataSet>

			<xsl:for-each select="//Comparision">

				<xsl:if test ="number(COL16)">

					<Comparision>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_SUFFIX" >
							<xsl:value-of select="COL28"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_SUFFIX">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBSuffixCode=$PB_SYMBOL_SUFFIX]/@TickerSuffixCode"/>
						</xsl:variable>
						

						<xsl:variable name = "PB_SYMBOL" >
							<xsl:value-of select="normalize-space(COL24)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:variable name="varOption">
							<xsl:call-template name="Option">
								<xsl:with-param name="varSymbol" select="COL27"/>
							</xsl:call-template>
						</xsl:variable>


						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>

								<xsl:when test="COL6='Option'">
									<xsl:value-of select="$varOption"/>
								</xsl:when>

								<xsl:when test="COL28='USD'">
									<xsl:value-of select ="normalize-space(COL13)"/>
								</xsl:when>
								<xsl:when test="COL28!='USD'">
									<xsl:value-of select ="''"/>
								</xsl:when>							

							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL28='USD'">
									<xsl:value-of select ="''"/>
								</xsl:when>
								<xsl:when test="COL28!='USD'">
									<xsl:value-of select ="normalize-space(COL14)"/>
								</xsl:when>

							</xsl:choose>
						</SEDOL>

						<SMRequest>
							<xsl:value-of select="'True'"/>
						</SMRequest>

						<Asset>
							<xsl:value-of select ="COL6"/>
						</Asset>


						<CompanyName>
							<xsl:value-of select="normalize-space(COL27)"/>
						</CompanyName>

						<CurrencySymbol>
							<xsl:value-of select ="COL28"/>
						</CurrencySymbol>

						<xsl:variable name ="Quantity" select="number(COL16)"/>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="$Quantity &gt; 0">
									<xsl:value-of select ="$Quantity"/>
								</xsl:when>
								<xsl:when test ="$Quantity &lt; 0">
									<xsl:value-of select ="$Quantity*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="COL6='Option'">
									<xsl:choose>
										<xsl:when test="$varSide='Buy'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:when test="$varSide='SellShort'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
										<xsl:when test="$varSide='CoverShort'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSide='Buy'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:when test="$varSide='SellShort'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:when test="$varSide='CoverShort'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>



						<xsl:variable name ="varAvgPx">
							<xsl:value-of select ="number(COL18)"/>
						</xsl:variable>


						<AvgPX>
							<xsl:choose>
								<xsl:when test ="$varAvgPx &lt;0">
									<xsl:value-of select ="$varAvgPx*-1"/>
								</xsl:when>

								<xsl:when test ="$varAvgPx &gt;0">
									<xsl:value-of select ="$varAvgPx"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<TradeDate>
							<xsl:value-of select="COL7"/>
						</TradeDate>

						<xsl:variable name="varNotional">
							<xsl:value-of select="COL25"/>
						</xsl:variable>
						
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test ='number($varNotional) &lt; 0'>
									<xsl:value-of select ='$varNotional*-1'/>
								</xsl:when>

								<xsl:when test ='number($varNotional) &gt; 0'>
									<xsl:value-of select ='$varNotional'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="varCommision">
							<xsl:value-of select="COL19"/>
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

						<xsl:variable name="varFees">
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($varFees) &lt; 0'>
									<xsl:value-of select ='$varFees*-1'/>
								</xsl:when>

								<xsl:when test ='number($varFees) &gt; 0'>
									<xsl:value-of select ='$varFees'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="varStamp">
							<xsl:value-of select="COL20"/>
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
						</StampDuty>
						<xsl:variable name="varTransactionLevy">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<TransactionLevy>
							<xsl:choose>
								<xsl:when test ='number($varTransactionLevy) &lt; 0'>
									<xsl:value-of select ='$varTransactionLevy*-1'/>
								</xsl:when>

								<xsl:when test ='number($varTransactionLevy) &gt; 0'>
									<xsl:value-of select ='$varTransactionLevy'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionLevy>
						
					</Comparision>

				</xsl:if>

			</xsl:for-each>

		</NewDataSet>

	</xsl:template>

</xsl:stylesheet>
