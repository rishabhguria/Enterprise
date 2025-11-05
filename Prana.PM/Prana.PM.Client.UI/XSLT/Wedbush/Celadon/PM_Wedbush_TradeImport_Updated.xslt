<?xml version="1.0" encoding="utf-8" ?>
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


	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>



	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL35,'P') or contains(COL35,'C')">

			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of  select="normalize-space(COL36)"/>
			</xsl:variable>

			<xsl:variable name="varMonth">
				<xsl:value-of select="substring-before(COL37,'/')"/>
			</xsl:variable>

			<xsl:variable name="varDay">
				<xsl:value-of select="substring-before(substring-after(COL37,'/'),'/')"/>
			</xsl:variable>


			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(COL37,'/'),'/'),3)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="COL35"/>
			</xsl:variable>

			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(COL38 div 1,'##.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($varMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>

			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($varDay,1,1)='0'">
						<xsl:value-of select="substring($varDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name='varThirdFriday'>
				<xsl:value-of select='my:Now(number(concat("20",$ExpiryYear)),number($varMonth))'/>
			</xsl:variable>

			<xsl:choose>
				<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varDay - 1)">
					<xsl:value-of select="normalize-space(concat('O:', $UnderlyingSymbol, ' ', $ExpiryYear,$MonthCodeVar,$StrikePrice))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
				</xsl:otherwise>
			</xsl:choose>
		
		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL4"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Quantity)">

					<PositionMaster>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="COL18"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Wedbush']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>


						<xsl:variable name ="varCallPut">
							<xsl:choose>
								<xsl:when test="COL19 = '*' and COL5 != ''">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varUnderlying">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<!--<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="COL5 != '' and COL5 != '*' and $varCallPut = 1">
									<xsl:value-of select="substring-before(COL5,'1')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL5)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->


						<xsl:variable name="varAsset">
							<xsl:choose>
								<xsl:when test="contains(COL35,'C') or contains(COL35,'P')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:choose>
										<xsl:when test ="$PRANA_SYMBOL != ''">
											<xsl:value-of select ="$PRANA_SYMBOL"/>
										</xsl:when>
										<xsl:when test="$varCallPut = 1">
											<xsl:call-template name="Option">
												<xsl:with-param name="Symbol" select="COL5"/>
												<xsl:with-param name="Suffix" select="''"/>
											</xsl:call-template>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varUnderlying"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>
						<!--<Symbol>
				  
						  <xsl:choose>
							  <xsl:when test ="$PRANA_SYMBOL != ''">
								  <xsl:value-of select ="$PRANA_SYMBOL"/>
							  </xsl:when>
							  <xsl:when test="$varAsset = 'EquityOption'">
								  <xsl:call-template name="Option">
									  <xsl:with-param name="Symbol" select="COL5"/>
									  <xsl:with-param name="Suffix" select="''"/>
								  </xsl:call-template>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="$varUnderlying"/>
							  </xsl:otherwise>
						 
			      	  </xsl:choose>

			  </Symbol>-->

						<PBSymbol>
							<xsl:if test="COL5 != '' and COL5 != '*'">
								<xsl:value-of select="COL5"/>
							</xsl:if>
						</PBSymbol>


						<CostBasis>
							<xsl:choose>
								<xsl:when  test="number(COL6)">
									<xsl:value-of select="COL6"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</CostBasis>



						<PositionStartDate>
							<xsl:value-of select="COL28"/>
						</PositionStartDate>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number(normalize-space(COL4)) and COL4 &gt; 0">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:when test="number(normalize-space(COL4)) and COL4 &lt; 0">
									<xsl:value-of select="COL4 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL3 != '' and COL3 = 'P'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="COL3 != '' and COL3 = 'S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:when test="COL3 != '' and COL3 = 'SS'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="COL3 != '' and COL2 = 'ASG'">
									<xsl:value-of select="'A'"/>
								</xsl:when>

								<xsl:when test="COL3 != '' and COL2 = 'EXP'">
									<xsl:value-of select="'A'"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name ="varCommission">
							<xsl:choose>
								<xsl:when test ="number(COL15) ">
									<xsl:value-of select ="COL15"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varFees">
							<xsl:choose>
								<xsl:when test ="number(COL12) ">
									<xsl:value-of select ="COL12"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Commission>
							<xsl:choose>
								<xsl:when test ="number($varCommission)">
									<xsl:value-of select ="$varCommission"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>


						</Commission>

						<Fees>
							<xsl:choose>
								<xsl:when test ="number($varFees) and $varFees &gt; 0">
									<xsl:value-of select ="$varFees"/>
								</xsl:when>
								<xsl:when test ="number($varFees) and $varFees &lt; 0">
									<xsl:value-of select ="$varFees * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>


						<OriginalPurchaseDate>
							<xsl:value-of select="COL28"/>
						</OriginalPurchaseDate>


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


