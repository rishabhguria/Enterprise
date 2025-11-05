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

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>
			<xsl:when  test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12  and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="varSymbol"/>

		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:value-of select ="substring-before($varSymbol,' ')"/>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test="string-length($varSymbol) &gt;15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),1,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test="string-length($varSymbol) &gt;15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),3,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test="string-length($varSymbol) &gt;15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),5,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test="string-length($varSymbol) &gt;15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),7,1)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceInt">
			<xsl:choose>
				<xsl:when test="string-length($varSymbol) &gt;15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),8,5)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceDec">
			<xsl:choose>
				<xsl:when test="string-length($varSymbol) &gt;15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),13,3)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test="number($varStrikePriceInt) or number($varStrikePriceDec)">
					<xsl:value-of select ="format-number(concat($varStrikePriceInt,'.',$varStrikePriceDec),'#.00')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:call-template name ="MonthCode">
				<xsl:with-param name ="varMonth" select ="number($varMonth)"/>
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

		<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="string-length($varSymbol) &gt;15 and number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>


		<xsl:choose>
			<xsl:when test="string-length($varSymbol) &gt;15">
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
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name ="varQuantity">
					<xsl:value-of select ="number(COL7)"/>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GreenOwl'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL15"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="varSymbol" select="COL14"/>					

						<xsl:variable name="PB_SUFFIX_NAME">
							
							<xsl:choose>
								<xsl:when test="string-length($varSymbol) &lt;15">
									<xsl:value-of select="substring-after(COL14,' ')"/>
								</xsl:when>
								
								<xsl:when test="string-length($varSymbol) &gt;15">
									<xsl:choose>
										<xsl:when test="contains(normalize-space(substring-before($varSymbol,'1')),' ')">
											<xsl:value-of select="substring-after(normalize-space(substring-before($varSymbol,'1')),' ')"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>					

						<xsl:variable name="varOptionOSI">
							<xsl:choose>
								
								<xsl:when test="string-length($varSymbol) &gt;15">
									<xsl:choose>
										
										<xsl:when test="contains(normalize-space(substring-before($varSymbol,'1')),' ')">
											<xsl:value-of select="translate($varSymbol,substring-after(normalize-space(substring-before($varSymbol,'1')),' '),'')"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="$varSymbol"/>
										</xsl:otherwise>
										
									</xsl:choose>
								</xsl:when>
								
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varOption">
							<xsl:call-template name="Option">
								<xsl:with-param name="varSymbol" select="$varOptionOSI"/>
							</xsl:call-template>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL14!='*'">
									<xsl:choose>
										
										<xsl:when test="string-length($varSymbol) &lt;15">
											<xsl:choose>
												<xsl:when test="$PRANA_SUFFIX_NAME!=''">
													<xsl:value-of select="normalize-space(concat(substring-before(COL14,' '),$PRANA_SUFFIX_NAME))"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$varSymbol"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:choose>
												
												<xsl:when test="contains(normalize-space(substring-before($varSymbol,'1')),' ')">
													<xsl:value-of select="concat($varOption,$PRANA_SUFFIX_NAME)"/>
												</xsl:when>
												
												<xsl:otherwise>
													<xsl:value-of select="$varOption"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>					

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<PositionStartDate>
							<xsl:value-of select="COL4"/>
						</PositionStartDate>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="translate(COL21,',','')"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varSide" select="COL10"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="string-length(COL14)&gt;15">
									<xsl:choose>
										<xsl:when  test="$varSide='BUY'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SHORT'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when  test="$varSide='COVER'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$varSide='BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SHORT'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when  test="$varSide='COVER'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name = "PB_BROKER_NAME" >
							<xsl:value-of select="COL32"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_BROKER_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker = $PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$PRANA_BROKER_NAME!=''">
									<xsl:value-of select="$PRANA_BROKER_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<xsl:variable name="Commission">
							<xsl:value-of select="translate(translate(COL22,',',''),'$','')"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($Commission) &lt; 0'>
									<xsl:value-of select ='$Commission*-1'/>
								</xsl:when>
								<xsl:when test ='number($Commission) &gt; 0'>
									<xsl:value-of select ='$Commission'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="Fees">
							<xsl:value-of select="translate(translate(COL24,',',''),'$','')"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($Fees) &lt; 0'>
									<xsl:value-of select ='$Fees*-1'/>
								</xsl:when>
								<xsl:when test ='number($Fees) &gt; 0'>
									<xsl:value-of select ='$Fees'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

