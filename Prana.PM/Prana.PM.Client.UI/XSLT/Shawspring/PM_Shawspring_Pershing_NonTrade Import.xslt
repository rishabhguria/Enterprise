<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL25"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash) and (COL32='CURRENCY' and COL4!='MONEY FUND REDEMPTION' and COL4!='MONEY FUND PURCHASE' and COL4!='ACTIVITY WITHIN YOUR ACCT')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varAccountName" select="'PHW006826'"/>
            
						<FundName>
						  <xsl:value-of select ="$varAccountName"/>
						</FundName>

						<xsl:variable name="Date" select="COL1"/>

            <Date>
							<xsl:value-of select="$Date"/>
						</Date>

            <xsl:variable name="varCurrencyName" select="COL24"/>

            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>



						<xsl:variable name="AbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Cash * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
            

            <xsl:variable name ="varTransactionSubTypeName" select="normalize-space(COL4)"/>
						<xsl:variable name="PRANA_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="contains($varTransactionSubTypeName,'FOREIGN CUSTODY FEE')">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'PES BILLING FEE')">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'SERVICE CHARGE')">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'WIRED FUNDS FEE')">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'PORTFOLIO PERFORMANCE - ONLINE PLUS')">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'FREE CREDIT BALANCE INTEREST CHARGE')">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'INT. CHARGED ON DEBIT BALANCES')">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>
                <xsl:when test="contains($varTransactionSubTypeName,'FEDERAL FUNDS RECEIVED')">
                  <xsl:value-of select="'First Republic Bank Cash'"/>
                </xsl:when>
                <xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<JournalEntries>
							<xsl:choose>
                <xsl:when test="contains(COL4,'FEDERAL FUNDS RECEIVED')">
                  <xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                   <xsl:when test="$Cash &lt; 0">
                    <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
                   </xsl:when>
                   <xsl:when test="$Cash &gt; 0">
                    <xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
                   </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
							</xsl:choose>
						</JournalEntries>

						<Description>
							<xsl:value-of select="COL4"/>
						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>