<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
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

	<xsl:template name="FormatDate">
		<xsl:param name="DateTime" />
		<!-- converts date time double number to 18/12/2009 -->

		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019" />
		</xsl:variable>

		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))" />
		</xsl:variable>

		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
		</xsl:variable>

		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
		</xsl:variable>

		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
		</xsl:variable>

		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))" />
		</xsl:variable>

		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
		</xsl:variable>

		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))" />
		</xsl:variable>

		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
		</xsl:variable>

		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
		</xsl:variable>

		<xsl:variable name ="varMonthUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nMonth) = 1">
					<xsl:value-of select ="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="nDayUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nDay) = 1">
					<xsl:value-of select ="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>

	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL10)"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:variable name="varDescription">
					<xsl:value-of select="normalize-space(COL5)"/>
				</xsl:variable>
				
				<xsl:if test="number($varAmount)">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>
						
						<xsl:variable name="varCash">
							<xsl:choose>
								<xsl:when test="$varAmount &gt;0">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="$varAmount*(-1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

            <xsl:variable name="PRANA_PRE_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="$varAmount &lt;0 and (contains(normalize-space(COL2),'INT. CHARGED ON DEBIT BALANCES'))">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>
                <xsl:when test="$varAmount &lt;0 and contains(normalize-space(COL2),'WIRED FUNDS FEE')">
                  <xsl:value-of select="'Other_Fees'"/>
                </xsl:when>
                <xsl:when test="$varAmount &lt;0 and contains(normalize-space(COL2),'ELECTRONIC TRANSACTION')">
                  <xsl:value-of select="'CASH_WDL'"/>
                </xsl:when>
                <xsl:when test="$varAmount &lt;0 and contains(normalize-space(COL2),'FEDERAL FUNDS SENT')">
                  <xsl:value-of select="'CASH_WDL'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            

           <xsl:variable name="PRANA_POST_ACRONYM_NAME">
             <xsl:choose>
               <xsl:when test="$varAmount &gt;0 and contains(normalize-space(COL2),'ELECTRONIC TRANSACTION')">
                 <xsl:value-of select="'CASH_DEP'"/>
               </xsl:when>
             </xsl:choose>
          </xsl:variable>
						
						<JournalEntries>
              <xsl:choose>
                <xsl:when test="$varAmount &lt; 0">
                  <xsl:value-of select="concat($PRANA_PRE_ACRONYM_NAME, ':' , $varCash , '|' , 'Cash', ':' , $varCash)"/>
                </xsl:when>
                <xsl:when test="$varAmount &gt; 0">
                  <xsl:value-of select="concat('Cash', ':' , $varCash , '|' , $PRANA_POST_ACRONYM_NAME, ':' , $varCash)"/>
                </xsl:when>
               
              </xsl:choose>
						</JournalEntries>
						
						<Description>
							<xsl:value-of select="$varDescription"/>
						</Description>
						
						<xsl:variable name="varTradeDate">
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="COL1"/>
							</xsl:call-template>
						</xsl:variable>
						
						<Date>
							<xsl:value-of select="$varTradeDate"/>
						</Date>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>